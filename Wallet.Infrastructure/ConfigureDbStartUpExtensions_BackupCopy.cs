using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Wallet.Infrastructure.Persistence;

namespace Wallet.Infrastructure;

public static class ConfigureDbStartUpExtensions_BackupCopy
{
    //public static async void MigrateDatabaseAndSeed(this IHost host)
    public static async void CreateWalletDbAndApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var _context = services.GetRequiredService<WalletDbContext>();
        var _logger = services.GetRequiredService<ILogger<WalletDbContext>>();


        //try
        //{

        //    await _context.Database.MigrateAsync();
        //    WalletDbContextInitialiser.SeedDataBase(_context);

        //    _logger.LogInformation("Seeding data into already created tables");



        //    //var databaseCreate = _context.Database.GetService<IDatabaseCreator>()
        //    //    as RelationalDatabaseCreator;

        //    //if (databaseCreate != null)
        //    //{
        //    //    _logger.LogInformation("About to create the database");
        //    //    if (!databaseCreate.CanConnect())
        //    //    {
        //    //        databaseCreate.Create();
        //    //        _logger.LogInformation("Creation of database because database is not available");
        //    //    }

        //    //    if (!databaseCreate.HasTables())
        //    //    {
        //    //        databaseCreate.CreateTables();
        //    //        _logger.LogInformation("Creation of tables in the database");
        //    //    }

        //    //    WalletDbContextInitialiser.InitializeDataBase(_context);


        //    //    _logger.LogInformation("Seeding data into already created tables");
        //    //}
        //}
        //catch (Exception ex)
        //{

        //    _logger.LogError($"migration issue {ex.Message}");
        //}






        // SECOND ALTERNATIVE
        _logger.LogInformation("MigrateDatabaseAndSeedAsync started");
        try
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                    .WaitAndRetry(
                        retryCount: 5,
                        // 2 secs, 4, 8, 16, 32 
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, retryCount, context) =>
                        {
                            _logger.LogError("Retrying MigrateDatabaseAndSeed {RetryCount} of {ContextPolicyKey} at {ContextOperationKey}, due to: {Exception}", retryCount, context.PolicyKey,
                                context.OperationKey, exception);
                        });

            //retryPolicy.Execute(MigrateAndSeed);

            await retryPolicy.Execute(async () =>
            {
                // these are just the essentials
                await _context.Database.MigrateAsync();
                WalletDbContextInitialiser.SeedDataBase(_context);
                _logger.LogInformation("Seeding data into already created tables");

            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }

        _logger.LogInformation("MigrateDatabaseAndSeedAsync completed");


    }

    //private static async void MigrateAndSeed()
    //{

    //    // these are just the essentials
    //    await _context.Database.MigrateAsync();
    //    WalletDbContextInitialiser.SeedDataBase(_context);
    //    _logger.LogInformation("Seeding data into already created tables");

    //}

















}
