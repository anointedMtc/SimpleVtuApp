using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ConfirmEmailVtuNation;

internal sealed class ConfirmEmailVtuNationValidator : AbstractValidator<ConfirmEmailVtuNationCommand>
{
    public ConfirmEmailVtuNationValidator()
    {

        RuleFor(r => r.ConfirmEmailRequestVtuNation.VerificationId)
           .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");


    }
}
