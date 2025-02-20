using DomainSharedKernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;
using VtuApp.Infrastructure.ExternalServices.VtuNationApi;
using VtuApp.Infrastructure.Persistence.Repositories;

namespace VtuApp.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddVtuAppInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(VtuAppRepository<>));


        // REFIT
        services.AddRefitClient<IGetTokenFromVtuNation>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.vtunation.com"))
                .AddStandardResilienceHandler();

        services.AddTransient<ITokenStoreForVtuNation, TokenStoreForVtuNation>();
        services.AddTransient<AuthHeaderHandlerForVtuNation>();

        services.AddRefitClient<IGetAdminServicesFromVtuNation>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.vtunation.com"))
                .AddHttpMessageHandler<AuthHeaderHandlerForVtuNation>()
                .AddStandardResilienceHandler();

        services.AddRefitClient<IGetServicesFromVtuNation>()
               .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.vtunation.com"))
               .AddHttpMessageHandler<AuthHeaderHandlerForVtuNation>()
               .AddStandardResilienceHandler();


        return services;
    }
}
