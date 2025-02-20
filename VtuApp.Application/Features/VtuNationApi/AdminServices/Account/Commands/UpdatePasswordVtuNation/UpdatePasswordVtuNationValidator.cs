using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdatePasswordVtuNation;

internal sealed class UpdatePasswordVtuNationValidator : AbstractValidator<UpdatePasswordVtuNationCommand>
{
    public UpdatePasswordVtuNationValidator()
    {
        RuleFor(r => r.UpdatePasswordRequestVtuNation.OtpToken)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.UpdatePasswordRequestVtuNation.NewPassword)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
