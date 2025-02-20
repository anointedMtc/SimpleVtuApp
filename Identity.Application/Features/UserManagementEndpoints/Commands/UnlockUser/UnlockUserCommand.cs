using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UnlockUser;

public class UnlockUserCommand : IRequest<UnlockUserResponse>
{
    public Guid UserId { get; set; }
}
