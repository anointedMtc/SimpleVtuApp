using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenCommand : IRequest<RevokeRefreshTokenResponse>
{
    public RevokeRefreshTokenRequestDto RevokeRefreshTokenRequestDto { get; set; }
}
