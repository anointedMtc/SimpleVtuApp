namespace Wallet.Shared.DTO;

public class OwnerDto
{
    public Guid OwnerId { get; set; }  
    public string Email { get; set; }   
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
