using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaOrchestrationStateMachines.Common.Entities;
using SagaOrchestrationStateMachines.VtuAirtimeOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.VtuDataOrderedSagaOrchestrator;

namespace SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator;

public sealed class SagaStateMachineDbContext : SagaDbContext
{
    public SagaStateMachineDbContext(DbContextOptions<SagaStateMachineDbContext> options) : base(options)
    {
    }

    public DbSet<SagaStateMachineEntity> SagaStateMachineEntities { get; set; }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { 
                yield return new UserCreatedSagaStateMap();

                yield return new VtuAirtimeOrderedSagaStateMap();

                yield return new VtuDataOrderedSagaStateMap();

        }

    }
}
