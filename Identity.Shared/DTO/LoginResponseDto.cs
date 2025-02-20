namespace Identity.Shared.DTO;

public class LoginResponseDto
{
    public bool IsAuthSuccessful { get; set; }
    // I always use these two properties in my authentication response to the client becuase on the client app, I have to decide about the authentication flow as well... and if two factor is required, I will redirect the user to the TwoFactor page to enter the code and I will pass the provider to that page as a query string parameter
    public bool Is2FactorRequired { get; set; }
    public string? Provider { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public int TokenExpiresInSeconds { get; set; }

    // expiresIn: 3600  should only be be populated when loggin in... and it is just the constant time/duration of our token's lifespan... 

    //public string? ErrorMessage { get; set; }

}
