using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.RemoveUserClaim;

public class RemoveUserClaimCommand : IRequest<RemoveUserClaimResponse>
{
    public Guid UserId { get; set; }
    public RemoveUserClaimRequestDto RemoveUserClaimRequestDto { get; set; }
}
