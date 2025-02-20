using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Earnings.Commands.TransferBonusToMainWalletVtuNation;

internal sealed class TransferBonusToMainWalletVtuNationValidator : AbstractValidator<TransferBonusToMainWalletVtuNationCommand>
{
    public TransferBonusToMainWalletVtuNationValidator()
    {
        RuleFor(r => r.TransferBonusToMainWalletRequestVtuNation.Amount)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements")
           .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonProperty}.");

       
    }
}
