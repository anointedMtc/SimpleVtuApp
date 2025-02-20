using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<DeleteUserResponse>
{
    public Guid UserId { get; set; }
}
