using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuDataSaga;

public record RollbackAmountForVtuDataPurchaseFailedMessage(
    Guid ApplicationUserId,
    string Email,
    string FirstName,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    string DataPlanPurchased,
    decimal AmountPurchased,
    decimal PricePaid,
    string Reciever,
    decimal InitialBalance,
    decimal FinalBalance,
    DateTimeOffset CreatedAt);
