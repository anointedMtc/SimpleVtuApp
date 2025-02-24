using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaOrchestrationStateMachines.Domain.Entities;
using SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;
using SagaOrchestrationStateMachines.Infrastructure.VtuAirtimeOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.Infrastructure.VtuDataOrderedSagaOrchestrator;

namespace SagaOrchestrationStateMachines.Infrastructure.Persistence;

public class SagaStateMachineDbContext : SagaDbContext
{
    public SagaStateMachineDbContext(DbContextOptions<SagaStateMachineDbContext> options) : base(options)
    {
    }

    public DbSet<SagaStateMachineEntity> SagaStateMachineEntities { get; set; }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new UserCreatedSagaStateMap();

            yield return new VtuAirtimeOrderedSagaStateMap();

            yield return new VtuDataOrderedSagaStateMap();

        }

    }
}
