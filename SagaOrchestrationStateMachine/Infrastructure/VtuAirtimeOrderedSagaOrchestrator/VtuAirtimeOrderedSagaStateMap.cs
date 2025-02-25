using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrationStateMachines.Infrastructure.VtuAirtimeOrderedSagaOrchestrator;

public sealed class VtuAirtimeOrderedSagaStateMap : SagaClassMap<VtuAirtimeOrderedSagaStateInstance>
{
    protected override void Configure(EntityTypeBuilder<VtuAirtimeOrderedSagaStateInstance> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);

        entity.Property(x => x.ApplicationUserId).HasMaxLength(64);

        entity.Property(x => x.AmountToPurchase).HasColumnType("decimal (18,2)");

        entity.Property(x => x.PricePaid).HasColumnType("decimal (18,2)");
       
        entity.Property(x => x.InitialBalance).HasColumnType("decimal (18,2)");
        
        entity.Property(x => x.FinalBalance).HasColumnType("decimal (18,2)");


        entity.Property(x => x.RowVersion).IsRowVersion();
    }
}
