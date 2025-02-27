namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.UserCreatedSaga;

// chose to do it this way becuase of the way we are initializing the scheduler to avoid uninitialized members in the constructor when created
//public record NotifyApplicationUserOfWalletCreatedEvent
//{
//    public Guid ApplicationUserId { get; init; }
//    public string FirstName { get; init; }
//    public string LastName { get; init; }
//    public string Email { get; init; }
//    public decimal BonusBalance { get; init; }

//}



public record NotifyApplicationUserOfWalletCreatedEvent(
    Guid ApplicationUserId,
    string FirstName,
    string LastName,
    string Email,
    decimal BonusBalance);

