namespace Wallet.Shared.DTO;

public class WalletDto
{
    public Guid OwnerId { get; set; }
    public Guid WalletId { get; set; }
    public Guid ApplicationUserId { get; set; }
    public string UserEmail { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal WalletBalance { get; set; }
    public List<TransferDto> Transfers { get; set; }
}
