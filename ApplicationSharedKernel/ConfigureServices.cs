using ApplicationSharedKernel.Behaviours;
using ApplicationSharedKernel.Interfaces;
using ApplicationSharedKernel.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Services;
using System.Reflection;

namespace ApplicationSharedKernel;

public static class ConfigureServices
{
    public static IServiceCollection AddSharedKernelApplicationServices(this IServiceCollection services)
    {

        services.AddTransient<IDateTimeService, DateTimeService>();

        services.AddScoped<IResourceBaseAuthorizationService, ResourceBaseAuthorizationService>();

        services.AddScoped<IUserContext, UserContext>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            config.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));

            config.AddOpenBehavior(typeof(LoggingBehaviour<,>));

            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));

            config.AddOpenBehavior(typeof(QueryCachingBehaviour<,>));
            config.AddOpenBehavior(typeof(AdminQueryCachingBehaviour<,>));

            config.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
        });

        return services;
    }
}
