using Application.Models;

namespace Identity.Shared.DTO;


public class ApplicationUserResponseDto
{
    // the user Identity coming from the IdentityUser is a Guid Id which is a random string
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ConstUserName { get; set; }
    public string UserName { get; set; }
    public string Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool IsTwoFacAuthEnabled { get; set; }

    // expiresIn: 3600  should only be be populated when loggin in... and it is just the constant time/duration of our token's lifespan... 

    //public List<ApplicationRole> ApplicationRoles { get; set; }
    //public List<ApplicationRoleForUserResponseDto> UserRoles { get; set; }

    //public List<string?> UserRoles => ApplicationRoles.Select(x => x.Name).ToList();

}
