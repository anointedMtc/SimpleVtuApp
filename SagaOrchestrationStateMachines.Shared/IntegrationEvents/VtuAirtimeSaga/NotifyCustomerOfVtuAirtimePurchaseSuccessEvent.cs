using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;

public record NotifyCustomerOfVtuAirtimePurchaseSuccessEvent(
    Guid ApplicationUserId,
    string Email,
    string FirstName,
    string LastName,
    Guid VtuTransactionId,
    NetworkProvider NetworkProvider,
    decimal AmountPurchased,
    decimal PricePaid,
    string Reciever,
    string Sender,
    decimal InitialBalance,
    decimal FinalBalance,
    DateTimeOffset CreatedAt);
