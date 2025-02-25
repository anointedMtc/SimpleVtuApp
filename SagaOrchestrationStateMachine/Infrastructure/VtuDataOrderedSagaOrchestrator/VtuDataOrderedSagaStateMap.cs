using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrationStateMachines.Infrastructure.VtuDataOrderedSagaOrchestrator;

public sealed class VtuDataOrderedSagaStateMap : SagaClassMap<VtuDataOrderedSagaStateInstance>
{
    protected override void Configure(EntityTypeBuilder<VtuDataOrderedSagaStateInstance> entity, ModelBuilder model)
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
