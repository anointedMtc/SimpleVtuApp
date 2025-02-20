using DomainSharedKernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Infrastructure.Persistence;
using Wallet.Infrastructure.Persistence.Repositories;

namespace Wallet.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddWalletInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(WalletRepository<>));

        services.AddScoped<WalletDbContextInitialiser>();



        return services;
    }
}
