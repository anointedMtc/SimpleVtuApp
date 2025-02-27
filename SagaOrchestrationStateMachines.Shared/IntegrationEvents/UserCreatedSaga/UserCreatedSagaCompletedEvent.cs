namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.UserCreatedSaga;

public record UserCreatedSagaCompletedEvent
{
    public Guid ApplicationUserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public decimal BonusBalance { get; init; }

}