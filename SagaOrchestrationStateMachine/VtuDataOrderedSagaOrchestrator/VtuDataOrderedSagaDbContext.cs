using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrationStateMachines.VtuDataOrderedSagaOrchestrator;

public sealed class VtuDataOrderedSagaDbContext : SagaDbContext
{
    public VtuDataOrderedSagaDbContext(DbContextOptions<VtuDataOrderedSagaDbContext> options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new VtuDataOrderedSagaStateMap(); }
    }
}
