using FluentValidation;

namespace Notification.Application.Features.SendEmailToMultipleUsers;

public class SendEmailToMultipleUsersValidator : AbstractValidator<SendEmailToMultipleUsersCommand>
{
    public SendEmailToMultipleUsersValidator()
    {
        
    }
}
