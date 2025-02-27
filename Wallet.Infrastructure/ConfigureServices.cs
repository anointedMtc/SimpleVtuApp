using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Interfaces;
using Wallet.Domain.Interfaces;
using Wallet.Infrastructure.Persistence;
using Wallet.Infrastructure.Persistence.Repositories;

namespace Wallet.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddWalletInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WalletModuleDb") 
            ?? throw new InvalidOperationException("Connection string"
            + "'DefaultConnection' not found.");

        services.AddDbContext<WalletDbContext>(options =>
                options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure()));

        services.AddScoped(typeof(IWalletRepository<>), typeof(WalletRepository<>));

        services.AddScoped<WalletDbContextInitialiser>();



        return services;
    }
}
