using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.GenerateOtpVtuNation;

internal sealed class GenerateOtpVtuNationValidator : AbstractValidator<GenerateOtpVtuNationCommand>
{
    public GenerateOtpVtuNationValidator()
    {
        RuleFor(r => r.GenerateOtpRequestVtuNation.Type)
           .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
