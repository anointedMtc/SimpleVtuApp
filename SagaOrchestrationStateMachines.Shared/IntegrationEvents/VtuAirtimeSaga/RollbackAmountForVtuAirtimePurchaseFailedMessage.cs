using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;

public record RollbackAmountForVtuAirtimePurchaseFailedMessage(
    Guid ApplicationUserId,
    string Email,
    string FirstName,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    decimal AmountPurchased,
    decimal PricePaid,
    string Reciever,
    decimal InitialBalance,
    decimal FinalBalance,
    DateTimeOffset CreatedAt);
