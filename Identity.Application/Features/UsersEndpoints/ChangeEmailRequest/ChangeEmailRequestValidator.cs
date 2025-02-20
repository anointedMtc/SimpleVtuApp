using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.ChangeEmailRequest;

public class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequestCommand>
{
    public ChangeEmailRequestValidator()
    {
        //RuleFor(e => e..CurrentEmail)
        //    .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
        //    .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

        RuleFor(e => e.ChangeEmailRequestDto.NewEmail)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

    }
}
