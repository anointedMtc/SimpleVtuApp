namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.UserCreatedSaga;

public record NotifyApplicationUserOfWalletCreatedEvent(
    Guid ApplicationUserId,
    string FirstName,
    string LastName,
    string Email,
    decimal BonusBalance);

