using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SetUpdateTransactionPassVtuNation;

internal sealed class SetUpdateTransactionPassVtuNationValidator : AbstractValidator<SetUpdateTransactionPassVtuNationCommand>
{
    public SetUpdateTransactionPassVtuNationValidator()
    {
        RuleFor(r => r.SetUpdateTransactionPassRequestVtuNation.OldTransactionPass)
           .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.SetUpdateTransactionPassRequestVtuNation.NewTransactionPass)
           .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
