using Identity.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Notification.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.Infrastructure.Persistence;
using VtuApp.Infrastructure.Persistence;
using VtuHost.WebApi.Extensions.CustomImplementations;
using Wallet.Infrastructure.Persistence;

namespace VtuHost.WebApi.Extensions;

public static class HealthCheckExtension
{
    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHealthChecks()

         //.AddCheck<VtuNationApiHealthCheck>(nameof(VtuNationApiHealthCheck), failureStatus: HealthStatus.Unhealthy, tags: ["custom", "apiEndPoint", "vtuNationApi"])


         .AddDbContextCheck<WalletDbContext>()    
         .AddDbContextCheck<VtuDbContext>()    
         .AddDbContextCheck<EmailDbContext>()    
         .AddDbContextCheck<IdentityAuthDbContext>()    
         .AddDbContextCheck<SagaStateMachineDbContext>()    

         .AddSqlServer(configuration.GetConnectionString("IdentityModuleDb")!, name: "IdentityModuleDb-Sql Health", tags: ["database", "sqlServer", "custom", "itCanHaveMultipleNames"])   
         .AddSqlServer(configuration.GetConnectionString("NotificationModuleDb")!, name: "NotificationModuleDb-Sql Health", tags: ["database", "sqlServer", "custom", "itCanHaveMultipleNames"])    
         .AddSqlServer(configuration.GetConnectionString("VtuAppModuleDb")!, name: "VtuAppModuleDb-Sql Health", tags: ["database", "sqlServer", "custom", "itCanHaveMultipleNames"])    
         .AddSqlServer(configuration.GetConnectionString("WalletModuleDb")!, name: "WalletModuleDb-Sql Health", tags: ["database", "sqlServer", "custom", "itCanHaveMultipleNames"])
         .AddSqlServer(configuration.GetConnectionString("SagaStateMachinesModuleDb")!, name: "SagaStateMachinesModuleDb-Sql Health", tags: ["database", "sqlServer", "custom", "itCanHaveMultipleNames"]);

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
