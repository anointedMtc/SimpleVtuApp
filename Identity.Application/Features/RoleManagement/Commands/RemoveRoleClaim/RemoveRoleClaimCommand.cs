using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.RoleManagement.Commands.RemoveRoleClaim;

public class RemoveRoleClaimCommand : IRequest<RemoveRoleClaimResponse>
{
    public Guid AppRoleId { get; set; }

    public RemoveRoleClaimRequestDto RemoveRoleClaimRequestDto { get; set; }
}
