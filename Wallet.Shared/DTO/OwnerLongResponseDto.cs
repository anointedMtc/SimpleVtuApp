namespace Wallet.Shared.DTO;

public record OwnerLongResponseDto
{
    public Guid OwnerId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public WalletDto WalletDto { get; set; }

}
