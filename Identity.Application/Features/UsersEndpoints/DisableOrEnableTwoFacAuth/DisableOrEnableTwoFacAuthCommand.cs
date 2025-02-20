using MediatR;

namespace Identity.Application.Features.UsersEndpoints.DisableOrEnableTwoFacAuth;

public class DisableOrEnableTwoFacAuthCommand : IRequest<DisableOrEnableTwoFacAuthResponse>
{
    public bool IsTwoFacAuthEnabled { get; set; }

}
