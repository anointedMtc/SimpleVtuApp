using ExternalServices.Application.TypiCodeService.Interfaces;
using ExternalServices.Application.TypiCodeService.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Reflection;

namespace ExternalServices.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddExternalServicesApplicationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // REFIT
        services.AddRefitClient<IGetJWTFromExternalApiTwo>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"))
                .AddStandardResilienceHandler();

        services.AddTransient<IAuthTokenStoreExternalApi, AuthTokenStoreExternalApi>();
        services.AddTransient<ExternalApiAuthHeaderHandler>();

        services.AddRefitClient<IGetServiceFromExternalApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"))
            .AddHttpMessageHandler<ExternalApiAuthHeaderHandler>()
            .AddStandardResilienceHandler();

        return services;
    }
}
