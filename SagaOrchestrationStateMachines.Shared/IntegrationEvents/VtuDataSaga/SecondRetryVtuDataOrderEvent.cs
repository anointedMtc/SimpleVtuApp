using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuDataSaga;

public record SecondRetryVtuDataOrderEvent(
    Guid ApplicationUserId,
    string Email,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    string DataPlanPurchased,
    decimal AmountToPurchase,
    string Reciever);


//public record SecondRetryVtuDataOrderEvent
//{
//    public Guid ApplicationUserId { get; init; }
//    public string Email { get; init; }
//    public Guid VtuTransactionId { get; init; }
//    public NetworkProvider NetworkProvider { get; init; }
//    public string DataPlanPurchased { get; init; }
//    public decimal AmountToPurchase { get; init; }
//    public string Reciever { get; init; }
//}
