namespace Identity.Shared.DTO;

public class ApplicationUserShortResponseDto
{
    // the user Identity coming from the IdentityUser is a Guid Id which is a random string
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Gender { get; set; }
    public DateOfBirthResponseDto DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    //public List<ApplicationRoleForUserResponseDto> UserRoles { get; set; }

}
