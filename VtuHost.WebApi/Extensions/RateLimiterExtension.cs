using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Interfaces;
using System.Globalization;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace VtuHost.WebApi.Extensions;

public static class RateLimiterExtension
{
    public static IServiceCollection ConfigureRateLimiterServices(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var healthChecker = httpContext.RequestServices.GetRequiredService<HealthCheckService>();
                var healthStatus = healthChecker.CheckHealthAsync().ConfigureAwait(false).GetAwaiter().GetResult();         // The HealthCheckService.CheckHealthAsync() call is asynchronous, so we must make it synchronous, because we are in a synchronous context.

                if (healthStatus.Status == HealthStatus.Degraded)
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                         partitionKey: healthStatus.ToString()!,
                         factory: partition => new FixedWindowRateLimiterOptions
                         {
                             AutoReplenishment = true,                       
                             PermitLimit = 500,                              
                             QueueLimit = 25,                                 
                             Window = TimeSpan.FromMinutes(1),         
                             QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                         }
                    );
                }

                if (healthStatus.Status == HealthStatus.Unhealthy)
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                         partitionKey: healthStatus.ToString()!,
                         factory: partition => new FixedWindowRateLimiterOptions
                         {
                             AutoReplenishment = true,                       
                             PermitLimit = 100,                              
                             QueueLimit = 0,                                 
                             Window = TimeSpan.FromMinutes(1),         
                             QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                         }
                    );
                }

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.UserAgent.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,                       
                        PermitLimit = 1000,                              
                        QueueLimit = 50,                                 
                        Window = TimeSpan.FromMinutes(1),          
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }
                );

            });

            // PER-USER      --- Per Tenant
            options.AddPolicy("per-tenant", context =>
            {

                if (context.User.Identity?.IsAuthenticated is true)
                {
                    var existingUser = context.RequestServices.GetRequiredService<IUserContext>().GetCurrentUser()?.Email;

                    // User exists
                    // Get the user's rate limit from their plan
                    var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var userManager = context.RequestServices.GetService<UserManager<ApplicationUser>>();
                    var appUser = userManager?.FindByIdAsync(userId!).Result;
                    var rateLimit = appUser?.RateLimit ?? 0;

                    // Create a user-specific rate limiter that uses his/her name as partitionKey
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: existingUser!,
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = rateLimit,
                            QueueLimit = 0,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        }
                    );
                }

                // Fallback to host name for unauthenticated requests
                var nonExistingUser = context.Request.Headers.UserAgent.ToString();
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: nonExistingUser,
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 100,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }
                );
            });

            // Warning: The .Add{RateLimiter}() extension methods partition rate limits based on the policy name. This is okay if you want to apply global limits per group of endpoints, but it’s not when you want to partition per user or per IP address or something along those lines.
            // If you want to add policies that are partitioned by policy name and any aspect of an incoming HTTP request, use the .AddPolicy(..) method instead:
            options.AddPolicy("ApiFixed", httpContext =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.UserAgent.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,                                       
                        PermitLimit = 10,                                             
                        QueueLimit = 0,                                                 
                        Window = TimeSpan.FromMinutes(1),                         
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst        
                    }
                );
            });

            // to allow short bursts but constrain average rate, we could implement a sliding window algorithm...  This allows up to 100 requests per minute on average, with the ability to burst up to 25 additional requests which are queued.
            options.AddPolicy("ApiSliding", httpContext =>
            {
                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.UserAgent.ToString(),
                    factory: partition => new SlidingWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 100,
                        QueueLimit = 25,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 4,

                        // we are using the default options below, but you can configure it to your taste
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }
                );
            });

            options.AddPolicy("ApiTokenBucket", httpContext =>
            {
                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.UserAgent.ToString(),
                    factory: partition => new TokenBucketRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        TokenLimit = 100,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 25,
                        ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                        TokensPerPeriod = 20,
                    }
                );
            });

            options.AddPolicy("ApiConcurrency", httpContext =>
            {
                return RateLimitPartition.GetConcurrencyLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.UserAgent.ToString(),
                    factory: partition => new ConcurrencyLimiterOptions
                    {
                        PermitLimit = 100,
                        QueueLimit = 25,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    }
                );
            });

            options.OnRejected = (context, cancellationToken) =>
            {
                var userIdentity = context.HttpContext.User.Identity?.Name ?? context.HttpContext.Request.Headers.UserAgent.ToString();

                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);

                    context.HttpContext.Response.WriteAsync(
                        $"Request slots exceeded. Please try again after {retryAfter.TotalSeconds} second(s). " +
                        $"Read more about our rate limits at https://example.org/docs/ratelimiting.", cancellationToken);
                }
                else
                {
                    context.HttpContext.Response.WriteAsync("Request slots exceeded. Please try again later. Read more about our rate limits at https://example.org/docs/ratelimiting.", cancellationToken);
                }
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                // Given you have access to the current HttpContext, you also have access to the service collection. It’s a good practice to keep an eye on who, when and why a rate limit is being enforced, and you could log that by grabbing an ILogger from context.HttpContext.RequestServices if needed.
                context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
                .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
                .LogWarning("OnRejected: rate limit is being enforced for user with Id {userId}", userIdentity ?? "Un-Idnetified-User");

                return new ValueTask();
            };

            options.AddPolicy("NoLimit", ctx =>
            {
                return RateLimitPartition.GetNoLimiter("");
            });
            
        });

        return services;
    }
}
