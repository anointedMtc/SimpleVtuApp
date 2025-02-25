using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.UsersEndpoints.RefreshToken;

public class RefreshTokenResponse : ApiBaseResponse
{
    public RefreshTokenResponseDto RefreshTokenResponseDto { get; set; }
}
