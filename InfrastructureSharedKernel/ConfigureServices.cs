using ApplicationSharedKernel.Interfaces;
using InfrastructureSharedKernel.Caching;
using InfrastructureSharedKernel.Messaging;
using Microsoft.Extensions.DependencyInjection;

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
