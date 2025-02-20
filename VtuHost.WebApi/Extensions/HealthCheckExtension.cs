using Identity.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Notification.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator;
using SagaOrchestrationStateMachines.VtuAirtimeOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.VtuDataOrderedSagaOrchestrator;
using VtuApp.Infrastructure.Persistence;
using VtuHost.WebApi.Extensions.CustomImplementations;
using Wallet.Infrastructure.Persistence;

namespace VtuHost.WebApi.Extensions;

public static class HealthCheckExtension
{
    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHealthChecks()

         .AddCheck<VtuNationApiHealthCheck>(nameof(VtuNationApiHealthCheck), failureStatus: HealthStatus.Unhealthy, tags: ["custom", "apiEndPoint", "vtuNationApi"])


         .AddDbContextCheck<WalletDbContext>()    
         .AddDbContextCheck<VtuDbContext>()    
         .AddDbContextCheck<EmailDbContext>()    
         .AddDbContextCheck<ApplicationDbContext>()    
         .AddDbContextCheck<UserCreatedSagaDbContext>()    
         .AddDbContextCheck<VtuAirtimeOrderedSagaDbContext>()    
         .AddDbContextCheck<VtuDataOrderedSagaDbContext>()

         .AddSqlServer(configuration.GetConnectionString("cleanarchskeletonDb")!, name: "Sql Health", tags: ["database", "sqlServer", "custom", "itCanHaveMultipleNames"]);    

        services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(1200);     
            options.MaximumHistoryEntriesPerEndpoint(60);    
            options.SetApiMaxActiveRequests(1);    
            options.AddHealthCheckEndpoint("SimpleVtuApp Health Checks", "/api/healthcheck");  
        })
        .AddInMemoryStorage();
    }
}
