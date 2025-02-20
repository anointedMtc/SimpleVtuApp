using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.RoleManagement.Commands.AddRoleClaim;

public class AddRoleClaimCommand : IRequest<AddRoleClaimResponse>
{
    public Guid AppRoleId { get; set; }

    public AddRoleClaimRequestDto AddRoleClaimRequestDto { get; set; }
}
