namespace Identity.Shared.DTO;

public class VerifyTwoFacAuthRequestDto
{
    public string? Email { get; set; }

    // I have found a way to automatically supply the provider, so no need for this
    //public string? Provider { get; set; }

    public string? Token { get; set; }

    //public bool RememberMe { get; set; }
}
