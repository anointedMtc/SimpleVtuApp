using MediatR;

namespace Identity.Application.Features.UsersEndpoints.ResendEmailConfirmation;

public class ResendEmailConfirmationCommand : IRequest<ResendEmailConfirmationResponse>
{
    public string Email { get; set; }
}
