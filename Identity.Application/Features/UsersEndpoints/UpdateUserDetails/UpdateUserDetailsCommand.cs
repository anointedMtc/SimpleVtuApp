using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.UpdateUserDetails;

public class UpdateUserDetailsCommand : IRequest<UpdateUserDetailsResponse>
{
    public UpdateUserRequestDto UpdateUserRequestDto { get; set; }
}
