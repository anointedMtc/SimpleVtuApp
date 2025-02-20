namespace Identity.Shared.DTO;

public class ForgotPasswordRequestDto
{
    public string? Email { get; set; }

    //public string? ClientUri { get; set; }    // supply this as a private/static readonly prop/field
}
