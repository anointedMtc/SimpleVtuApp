using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Notification.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddNotificationApplicationLayer(this IServiceCollection services)
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
