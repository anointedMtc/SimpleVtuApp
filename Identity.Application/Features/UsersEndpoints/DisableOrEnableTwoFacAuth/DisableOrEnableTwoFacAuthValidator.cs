using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.DisableOrEnableTwoFacAuth;

public class DisableOrEnableTwoFacAuthValidator : AbstractValidator<DisableOrEnableTwoFacAuthCommand>
{
    public DisableOrEnableTwoFacAuthValidator()
    {
        //RuleFor(e => e.IsTwoFacAuthEnabled)
        //   .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

    }
}
