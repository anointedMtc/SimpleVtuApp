using MediatR;

namespace Identity.Application.Features.RoleManagement.Commands.DeleteApplicationRole;

public class DeleteApplicationRoleCommand : IRequest<DeleteApplicationRoleResponse>
{
    public Guid RoleId { get; set; }
}
