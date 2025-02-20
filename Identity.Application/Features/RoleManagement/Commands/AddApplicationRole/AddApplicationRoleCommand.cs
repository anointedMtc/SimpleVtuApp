using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.RoleManagement.Commands.AddApplicationRole;

public class AddApplicationRoleCommand : IRequest<AddApplicationRoleResponse>
{
    public AddApplicationRoleRequestDto AddApplicationRoleRequestDto { get; set; }
}
