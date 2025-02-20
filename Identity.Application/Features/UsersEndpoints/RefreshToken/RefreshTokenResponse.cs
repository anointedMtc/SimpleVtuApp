using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.UsersEndpoints.RefreshToken;

public class RefreshTokenResponse : ApiBaseResponse
{
    public RefreshTokenResponseDto RefreshTokenResponseDto { get; set; }
}
