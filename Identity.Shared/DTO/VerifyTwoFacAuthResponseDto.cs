namespace Identity.Shared.DTO;

public class VerifyTwoFacAuthResponseDto
{
    public bool IsAuthSuccessful { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public int TokenExpiresInSeconds { get; set; }
}
