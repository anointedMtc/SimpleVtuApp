using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.ChangeEmailRequest;

public class ChangeEmailRequestCommand : IRequest<ChangeEmailRequestResponse>
{
    public ChangeEmailRequestDto ChangeEmailRequestDto { get; set; }
}
