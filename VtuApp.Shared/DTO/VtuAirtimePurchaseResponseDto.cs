namespace VtuApp.Shared.DTO;

public record VtuAirtimePurchaseResponseDto
{
    public string TransactionType { get; init; }
    public string Receiver { get; init; }
    public decimal Amount { get; init; }
    public string NetworkProvider { get; init; }
    public string Sender { get; init; }
    public decimal InitialBalance { get; init; }
    public decimal FinalBalance { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public Guid VtuTransactionId { get; init; }
}
