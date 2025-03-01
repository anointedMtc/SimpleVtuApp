namespace Wallet.Shared.DTO;

public record WalletShortResponseDto
{
    public Guid OwnerId { get; set; }
    public Guid WalletId { get; set; }
    public Guid ApplicationUserId { get; set; }
    public string Email { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal WalletBalance { get; set; }
}
