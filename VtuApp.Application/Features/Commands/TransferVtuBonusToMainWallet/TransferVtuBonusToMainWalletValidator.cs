using FluentValidation;

namespace VtuApp.Application.Features.Commands.TransferVtuBonusToMainWallet;

internal sealed class TransferVtuBonusToMainWalletValidator : AbstractValidator<TransferVtuBonusToMainWalletCommand>
{
    public TransferVtuBonusToMainWalletValidator()
    {
        RuleFor(r => r.AmountToTransfer)
          .NotEmpty().WithMessage("{PropertyName} should have value. ")
          .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonProperty}.");

    }
}
