using DomainSharedKernel.Interfaces;
using MassTransit;

namespace SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;

public sealed class UserCreatedSagaStateInstance : SagaStateMachineInstance, IAggregateRoot
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public byte[] RowVersion { get; set; }


    public Guid ApplicationUserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public decimal RegisterationBonus { get; set; }
    public DateTimeOffset CreatedAt { get; set; }



    public int UserCreatedInAllModulesEventStatus { get; set; }


    public Guid? NotifyApplicationUserScheduleEventTokenId { get; set; }
}
