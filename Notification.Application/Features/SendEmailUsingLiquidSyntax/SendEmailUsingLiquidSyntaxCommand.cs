using MediatR;
using Notification.Shared.DTO;

namespace Notification.Application.Features.SendEmailUsingLiquidSyntax;

public class SendEmailUsingLiquidSyntaxCommand : IRequest<SendEmailUsingLiquidSyntaxResponse>
{
    public EmailDto EmailDto { get; set; }
    public UserEmailDto LiquidModel { get; set; }
    public string LiquidSyntax { get; set; }
}
