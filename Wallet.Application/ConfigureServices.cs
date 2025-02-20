using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Wallet.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddWalletApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
