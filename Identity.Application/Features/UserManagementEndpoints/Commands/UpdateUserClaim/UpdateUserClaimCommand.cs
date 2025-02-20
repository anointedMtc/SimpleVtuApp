using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UpdateUserClaim;

public class UpdateUserClaimCommand : IRequest<UpdateUserClaimResponse>
{
    public Guid UserId { get; set; }

    public UpdateUserClaimRequestDto UpdateUserClaimRequestDto { get; set; }
}
