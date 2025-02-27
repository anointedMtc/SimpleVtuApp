using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;

public record SecondRetryVtuAirtimeOrderEvent(
     Guid ApplicationUserId,
    string Email,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    decimal AmountToPurchase,
    string Reciever);





// chose to do it this way becuase of the way we are initializing the scheduler to avoid uninitialized members in the constructor when created
//public record SecondRetryVtuAirtimeOrderEvent
//{
//    public Guid ApplicationUserId { get; init; }
//    public string Email { get; init; }
//    public Guid VtuTransactionId { get; init; }
//    public NetworkProvider NetworkProvider { get; init; }
//    public decimal AmountToPurchase { get; init; }
//    public string Reciever { get; init; }
//}



