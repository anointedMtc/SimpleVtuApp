namespace VtuApp.Shared.DTO;

public record VtuDataPurchaseResponseDto
{
    public string TransactionType { get; init; }
    public string NetworkProvider { get; init; }
    public string Receiver { get; init; }
    public string DataPlan { get; init; }

    public string Label { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public decimal Discount { get; init; }
    public decimal PriceAfterDiscount { get; init; }

    public string Sender { get; init; }
    public decimal InitialBalance { get; init; }
    public decimal FinalBalance { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public Guid VtuTransactionId { get; init; }
}
