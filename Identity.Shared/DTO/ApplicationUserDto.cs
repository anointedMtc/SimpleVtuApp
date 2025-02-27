namespace Identity.Shared.DTO;

public class ApplicationUserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ConstUserName { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset LastLogin { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool IsTwoFacAuthEnabled { get; set; }
}
