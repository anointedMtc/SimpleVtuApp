using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ValidateOtpVtuNation;

internal sealed class ValidateOtpVtuNationValidator : AbstractValidator<ValidateOtpVtuNationCommand>
{
    public ValidateOtpVtuNationValidator()
    {
        RuleFor(r => r.ValidateOtpRequestVtuNation.Otp)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.ValidateOtpRequestVtuNation.Type)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
