namespace Identity.Shared.DTO;

public class ChangeEmailConfirmationRequestDto
{
    public string CurrentEmail { get; set; }
    public string NewEmail { get; set; }
    public string Token { get; set; }
}
