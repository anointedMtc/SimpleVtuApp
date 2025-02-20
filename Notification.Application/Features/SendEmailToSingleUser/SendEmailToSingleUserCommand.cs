using MediatR;
using Notification.Shared.DTO;

namespace Notification.Application.Features.SendEmailToSingleUser;

public class SendEmailToSingleUserCommand : IRequest<SendEmailToSingleUserResponse>
{
    public EmailDto EmailDto { get; set; }
}
