using FluentValidation;

namespace Notification.Application.Features.SendEmailToSingleUser;

public class SendEmailToSingleUserValidator : AbstractValidator<SendEmailToSingleUserCommand>
{
    public SendEmailToSingleUserValidator()
    {
        
    }
}
