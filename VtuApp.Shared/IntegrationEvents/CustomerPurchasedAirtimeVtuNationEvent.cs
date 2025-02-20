using VtuApp.Shared.Constants;

namespace VtuApp.Shared.IntegrationEvents;

public record CustomerPurchasedAirtimeVtuNationEvent
{
    public Guid CustomerId { get; init; }
    public Guid ApplicationUserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Guid VtuTransactionId { get; init; }
    public decimal AmountPurchased {  get; init; }
    public decimal PricePaid {  get; init; }
    public NetworkProvider NetworkProvider {  get; init; }
    public string Receiver { get; init; }
    public string Sender { get; init; }
    public decimal InitialBalance { get; init; }
    public decimal FinalBalance { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}
