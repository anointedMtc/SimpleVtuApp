using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.AssignUserRole;

public class AssignUserRoleCommand : IRequest<AssignUserRoleResponse>
{
    public AssignUserRoleRequestDto AssignUserRoleRequestDto { get; set; }
}
