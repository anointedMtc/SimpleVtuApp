using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.DeleteAllClaimsForAUser;

public class DeleteAllClaimsForAUserCommand : IRequest<DeleteAllClaimsForAUserResponse>
{
    public Guid UserId { get; set; }
}
