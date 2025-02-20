using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace VtuHost.WebApi.Middlewares;

public static class HealthChecksEndpointsMiddleware
{
    public static void MapHealthChecksEndpoints(this WebApplication app)
    {
        // healthChecks
        // this one will report for all the healthchecks in our system... unlike the one below that will filter it down to tags specified in the Predicate
        // One thing to note now that we have introduced multiple Health Checks is that the overall reported health status of our application will now depend on the combined status of our individual Health Checks. For example, if our database check is Healthy, but our custom check is Unhealthy, our system will report an Unhealthy status overall.
        app.MapHealthChecks("/api/healthcheck", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,       // This uses the built-in response writer from the Health Checks package, but we can create our own response writer should we wish to provide further information.

            ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
        });
        //.RequireAuthorization(PolicyNames.AdminAndAbove);
        //.RequireCors("MyHealthCorsPolicy");

        // this is now a new endpoint specifically for the ones tagged custom/database... think of it like "filters" or "grouping"
        app.MapHealthChecks("/api/healthcheck/custom", new HealthCheckOptions
        {
            // we are adding a predicate delegate to only return health checks that include our custom tag
            Predicate = reg => reg.Tags.Contains("custom"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,

            ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
        });

        // the default path is  /healthchecks-ui
        app.MapHealthChecksUI(options => options.UIPath = "/dashboard");


        // MASS TRANSIT
        // The AddMassTransit method adds an IHealthCheck to the service collection that you can use to monitor your health. The health check is added with the tags ready and masstransit. To configure health checks, map the ready and live endpoints in your ASP.NET application.
        // When everything works correctly, MassTransit will report Healthy. If any problems occur on application startup, MassTransit will report Unhealthy. This can cause an orcestrator to restart your application. If any problems occur while the application is working (for example, application loses connection to broker), MassTransit will report Degraded.
        app.MapHealthChecks("/health/ready", new HealthCheckOptions()
        {
            Predicate = (check) => check.Tags.Contains("ready"),
        });

        app.MapHealthChecks("/health/live", new HealthCheckOptions());


    }
}
