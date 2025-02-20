using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.VerifyTwoFacAuth;

public class VerifyTwoFacAuthCommand : IRequest<VerifyTwoFacAuthResponse>
{
    public VerifyTwoFacAuthRequestDto TwoFactorDto { get; set; }
}
