using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.LogInVtuNation;

internal sealed class LogInVtuNationValidator : AbstractValidator<LogInVtuNationCommand>
{
    public LogInVtuNationValidator()
    {
        RuleFor(r => r.LoginRequestVtuNation.Password)
      .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        
        RuleFor(r => r.LoginRequestVtuNation.Phone)
           .NotEmpty().WithMessage("{PropertyName} should have value.")
           .MinimumLength(11).WithMessage("{PropertyName} should me minimum of {ComparisonValue}. {PropertyValue} does not meet requirement.");

    }
}
