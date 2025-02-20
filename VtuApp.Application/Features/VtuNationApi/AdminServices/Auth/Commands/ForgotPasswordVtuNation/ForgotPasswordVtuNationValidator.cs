using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.ForgotPasswordVtuNation;

internal sealed class ForgotPasswordVtuNationValidator : AbstractValidator<ForgotPasswordVtuNationCommand>
{
    public ForgotPasswordVtuNationValidator()
    {
        RuleFor(r => r.ForgotPasswordRequestVtuNation.PhoneNumber)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
