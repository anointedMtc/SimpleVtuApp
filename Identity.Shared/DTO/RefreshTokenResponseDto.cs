namespace Identity.Shared.DTO;

public class RefreshTokenResponseDto
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public int TokenExpiresInSeconds { get; set; }

    // expiresIn: 3600  should only be be populated when loggin in... and it is just the constant time/duration of our token's lifespan... 

}
