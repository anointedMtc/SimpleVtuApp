using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Identity.Infrastructure;

public static class ConfigureIdentityDbStartUpExtensions
{
    public static async void CreateIdentityDbAndApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var _context = services.GetRequiredService<ApplicationDbContext>();
        // for logging
        var hostEnvironment = services.GetService<IHostEnvironment>();
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var _logger = loggerFactory.CreateLogger<ApplicationDbContext>();

        var _dbSeeder = services.GetRequiredService<ApplicationDbContextInitializer>();

        // SECOND ALTERNATIVE
        _logger.LogInformation($"MigrateDatabaseAndSeedAsync Starting in environment {hostEnvironment?.EnvironmentName}");
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

                await _dbSeeder.SeedIdentityData();

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
