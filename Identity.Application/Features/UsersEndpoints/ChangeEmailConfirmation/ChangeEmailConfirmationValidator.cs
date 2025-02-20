using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.ChangeEmailConfirmation;

public class ChangeEmailConfirmationValidator : AbstractValidator<ChangeEmailConfirmationCommand>
{
    public ChangeEmailConfirmationValidator()
    {
        RuleFor(e => e.ChangeEmailConfirmationRequestDto.NewEmail)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

        RuleFor(e => e.ChangeEmailConfirmationRequestDto.Token)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

        RuleFor(e => e.ChangeEmailConfirmationRequestDto.CurrentEmail)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");


    }
}
