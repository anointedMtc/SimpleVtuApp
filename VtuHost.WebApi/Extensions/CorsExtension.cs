using VtuHost.WebApi.Constants;

namespace VtuHost.WebApi.Extensions;

public static class CorsExtension
{
    public static void ConfigureCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicyNames.AllowAnyOrigin,
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyOrigin();
                    corsPolicyBuilder.AllowAnyMethod();
                    corsPolicyBuilder.AllowAnyHeader();
                }
            );
        });
    }
}
