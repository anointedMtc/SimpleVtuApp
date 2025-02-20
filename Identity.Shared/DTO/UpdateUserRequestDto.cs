namespace Identity.Shared.DTO;

public class UpdateUserRequestDto
{

    public string FirstName { get; set; }
    public string LastName { get; set; }
    //public string Email { get; set; }
    public string UserName { get; set; }
    public string Gender { get; set; }
    //public DateOnly? DateOfBirth { get; set; }
    public DateOfBirthResponseDto DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    //public bool IsTwoFacAuthEnabled { get; set; }

}
