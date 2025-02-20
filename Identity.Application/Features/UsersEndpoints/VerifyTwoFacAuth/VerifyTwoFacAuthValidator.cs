using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.VerifyTwoFacAuth;

public class VerifyTwoFacAuthValidator : AbstractValidator<VerifyTwoFacAuthCommand>
{
    public VerifyTwoFacAuthValidator()
    {
        RuleFor(e => e.TwoFactorDto.Email)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

        RuleFor(e => e.TwoFactorDto.Token)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

        //RuleFor(e => e.TwoFactorDto.Provider)
        //    .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

    }
}
