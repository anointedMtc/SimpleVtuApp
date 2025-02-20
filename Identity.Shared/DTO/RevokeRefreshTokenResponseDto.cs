namespace Identity.Shared.DTO;

public class RevokeRefreshTokenResponseDto
{
    // when a user log's out, you may decide to remove the refresh token (eventHandlers)

    public string Message { get; set; }

}
