namespace Identity.Shared.DTO;

public class ResetPasswordRequestDto
{
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
}
