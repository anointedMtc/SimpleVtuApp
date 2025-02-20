namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;

public record DeductFundsFromCustomerWalletForVtuPurchaseTransactionMessage(
    Guid ApplicationUserId,
    string Email,
    Guid VtuTransactionId,
    decimal PricePaid);
