using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SharedKernel.Domain.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;
using VtuApp.Infrastructure.ExternalServices.VtuNationApi;
using VtuApp.Infrastructure.Persistence;
using VtuApp.Infrastructure.Persistence.Repositories;

namespace VtuApp.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddVtuAppInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("VtuAppModuleDb")
            ?? throw new InvalidOperationException("Connection string"
            + "'DefaultConnection' not found.");

        services.AddDbContext<VtuDbContext>(options =>
                options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure()));

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
