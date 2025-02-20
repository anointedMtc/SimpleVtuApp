using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;

public record BuyAirtimeForCustomerMessage(
    Guid ApplicationUserId,
    string Email,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    decimal AmountToPurchase,
    string Reciever);
