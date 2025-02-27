using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;


//public record NotifyCustomerOfVtuAirtimePurchaseSuccessEvent
//{
//    public Guid ApplicationUserId { get; init; }
//    public string Email {  get; init; }
//    public string FirstName {  get; init; }
//    public string LastName {  get; init; }
//    public Guid VtuTransactionId { get; init; }
//    public NetworkProvider NetworkProvider { get; init; }
//    public decimal AmountPurchased { get; init; }
//    public decimal PricePaid { get; init; }
//    public string Reciever { get; init; }
//    public string Sender { get; init; }
//    public decimal InitialBalance { get; init; }
//    public decimal FinalBalance { get; init; }
//    public DateTimeOffset CreatedAt { get; init; }

//}


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
