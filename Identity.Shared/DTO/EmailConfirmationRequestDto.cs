namespace Identity.Shared.DTO;

public class EmailConfirmationRequestDto
{
    public string Email { get; set; }
    public string Token { get; set; }
}
