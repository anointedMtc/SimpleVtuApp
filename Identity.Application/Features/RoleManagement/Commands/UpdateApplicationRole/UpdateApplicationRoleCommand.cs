using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.RoleManagement.Commands.UpdateApplicationRole;

public class UpdateApplicationRoleCommand : IRequest<UpdateApplicationRoleResponse>
{
    public Guid RoleId { get; set; }

    public UpdateApplicationRoleRequestDto UpdateApplicationRoleRequestDto { get; set; }
}
