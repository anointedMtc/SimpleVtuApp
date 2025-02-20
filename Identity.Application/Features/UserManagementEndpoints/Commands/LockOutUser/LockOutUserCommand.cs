using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.LockOutUser;

public class LockOutUserCommand : IRequest<LockOutUserResponse>
{
    public Guid UserId { get; set; }
}
