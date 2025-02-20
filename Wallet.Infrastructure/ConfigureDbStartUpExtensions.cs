using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Wallet.Infrastructure.Persistence;

namespace Wallet.Infrastructure;

public static class ConfigureDbStartUpExtensions
{
    public static async void CreateWalletDbAndApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var _context = services.GetRequiredService<WalletDbContext>();
        // for logging
        var hostEnvironment = services.GetService<IHostEnvironment>();
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var _logger = loggerFactory.CreateLogger<WalletDbContext>();

        var _dbSeeder = services.GetRequiredService<WalletDbContextInitialiser>();

        // SECOND ALTERNATIVE
        _logger.LogInformation("MigrateDatabaseAndSeedAsync Starting in environment {nameOfEnvironment}", hostEnvironment?.EnvironmentName);
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
                    }
                );
            await retryPolicy.Execute(async () =>
            {
                await _context.Database.MigrateAsync();
                
                //WalletDbContextInitialiser.SeedDataBase(_context);
                _dbSeeder.SeedDataBase();
                
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
}
