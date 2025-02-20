using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<UpdateUserResponse>
{
    public Guid UserId { get; set; }
    public UpdateUserRequestDto UpdateUserRequestDto { get; set; }
}
