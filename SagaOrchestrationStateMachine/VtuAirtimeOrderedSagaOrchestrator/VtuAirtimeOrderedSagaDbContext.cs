//using MassTransit.EntityFrameworkCoreIntegration;
//using Microsoft.EntityFrameworkCore;

//namespace SagaOrchestrationStateMachines.VtuAirtimeOrderedSagaOrchestrator;

//public sealed class VtuAirtimeOrderedSagaDbContext : SagaDbContext
//{
//    public VtuAirtimeOrderedSagaDbContext(DbContextOptions<VtuAirtimeOrderedSagaDbContext> options) : base(options)
//    {
//    }

//    protected override IEnumerable<ISagaClassMap> Configurations
//    {
//        get { yield return new VtuAirtimeOrderedSagaStateMap(); }
//    }
//}
