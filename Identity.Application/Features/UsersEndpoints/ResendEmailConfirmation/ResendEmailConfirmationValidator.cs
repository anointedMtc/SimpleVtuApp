using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.ResendEmailConfirmation;

public class ResendEmailConfirmationValidator : AbstractValidator<ResendEmailConfirmationCommand>
{
    public ResendEmailConfirmationValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

    }
}
