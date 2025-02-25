using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Interfaces;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Messaging;

namespace SharedKernel.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddSharedKernelInfrastructureServices(this IServiceCollection services)
    {

        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheServiceRedis, CacheServiceRedis>();

        services.AddScoped<IMassTransitService, MassTransitService>();

        return services;
    }

}
