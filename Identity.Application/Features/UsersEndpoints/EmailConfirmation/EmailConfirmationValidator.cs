using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.EmailConfirmation;

public class EmailConfirmationValidator : AbstractValidator<EmailConfirmationCommand>
{
    public EmailConfirmationValidator()
    {

        RuleFor(e => e.EmailConfirmation.Email)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

        RuleFor(e => e.EmailConfirmation.Token)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

    }
}
