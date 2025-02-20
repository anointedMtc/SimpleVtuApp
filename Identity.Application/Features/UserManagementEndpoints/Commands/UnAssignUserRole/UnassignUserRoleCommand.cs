using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UnAssignUserRole;

public class UnassignUserRoleCommand : IRequest<UnassignUserRoleResponse>
{
    public UnassignUserRoleRequestDto UnassignUserRoleRequestDto { get; set; }
}
