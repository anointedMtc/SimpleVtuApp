using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuDataSaga;

public record NotifyCustomerOfVtuDataPurchaseSuccessEvent(
    Guid ApplicationUserId,
    string Email,
    string FirstName,
    string LastName,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    string DataPlanPurchased,
    decimal AmountPurchased,
    decimal PricePaid,
    string Reciever,
    string Sender,
    decimal InitialBalance,
    decimal FinalBalance,
    DateTimeOffset CreatedAt);