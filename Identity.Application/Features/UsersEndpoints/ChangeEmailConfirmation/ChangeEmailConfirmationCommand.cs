using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.ChangeEmailConfirmation;

public class ChangeEmailConfirmationCommand : IRequest<ChangeEmailConfirmationResponse>
{
    public ChangeEmailConfirmationRequestDto ChangeEmailConfirmationRequestDto { get; set; }
}
