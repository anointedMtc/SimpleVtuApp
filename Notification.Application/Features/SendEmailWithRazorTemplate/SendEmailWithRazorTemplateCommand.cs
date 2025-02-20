using MediatR;
using Notification.Shared.DTO;

namespace Notification.Application.Features.SendEmailWithRazorTemplate;

public class SendEmailWithRazorTemplateCommand : IRequest<SendEmailWithRazorTemplateResponse>
{
    public EmailDto EmailDto { get; set; }
    public UserEmailDto RazorModel { get; set; }
    public string RazorFilePath { get; set; }
}
