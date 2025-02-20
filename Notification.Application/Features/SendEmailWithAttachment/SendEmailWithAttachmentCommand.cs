using MediatR;
using Notification.Shared.DTO;

namespace Notification.Application.Features.SendEmailWithAttachment;

public class SendEmailWithAttachmentCommand : IRequest<SendEmailWithAttachmentResponse>
{
    public EmailDto EmailDto { get; set; }
}
