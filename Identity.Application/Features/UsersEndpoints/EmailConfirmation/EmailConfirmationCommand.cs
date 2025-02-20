using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.EmailConfirmation;

public class EmailConfirmationCommand : IRequest<EmailConfirmationResponse>
{
    public EmailConfirmationRequestDto EmailConfirmation { get; set; }
}
