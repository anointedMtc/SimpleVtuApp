namespace Wallet.Shared.DTO;

public class TransferDto
{
    public Guid TransferId { get; set; }
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
    public int Direction { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid ReferenceId { get; set; }
    public string ReasonWhy { get; set; }
}

