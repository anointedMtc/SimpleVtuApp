namespace Identity.Shared.DTO;

public class RevokeRefreshTokenRequestDto
{
    // this should be able to work with supplying a users Name/Email and Token as well
    // Because the management could want to revoke a refresh token for a particular user... alternatively, maybe they should lockOut the user
    //public string? Email { get; set; }
    public string RefreshToken { get; set; }
}
