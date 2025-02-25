using Hangfire;

namespace VtuHost.WebApi.Extensions;

public static class HangfireExtension
{
    public static void ConfigureHangfireBackgroundService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("NotificationModuleDb"));

        });
        services.AddHangfireServer();
    }
}
