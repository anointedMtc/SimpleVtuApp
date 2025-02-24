using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;

public sealed class UserCreatedSagaStateMap : SagaClassMap<UserCreatedSagaStateInstance>
{
    protected override void Configure(EntityTypeBuilder<UserCreatedSagaStateInstance> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);

        entity.Property(x => x.ApplicationUserId).HasMaxLength(64);



        entity.Property(x => x.RowVersion).IsRowVersion();
    }
}
