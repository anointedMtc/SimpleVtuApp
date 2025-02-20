using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.AddUserClaim;

public class AddUserClaimCommand : IRequest<AddUserClaimResponse>
{
    public Guid UserId { get; set; }

    public AddUserClaimRequestDto AddUserClaimRequestDto { get; set; }
}
