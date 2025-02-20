using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuDataSaga;

public record BuyDataForCustomerMessage(
    Guid ApplicationUserId,
    string Email,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    string DataPlanPurchased,
    decimal AmountToPurchase,
    string Reciever);