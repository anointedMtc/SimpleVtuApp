using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator;

public sealed class UserCreatedSagaDbContext : SagaDbContext
{
    public UserCreatedSagaDbContext(DbContextOptions<UserCreatedSagaDbContext> options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new UserCreatedSagaStateMap(); }
    }
}
