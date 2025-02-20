using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.RefreshToken;

public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    public RefreshTokenRequestDto RefreshTokenRequest { get; set; }
}
