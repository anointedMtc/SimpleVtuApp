using FluentValidation;

namespace Wallet.Application.Features.Commands.TransferFunds;

public class TransferFundsValidator : AbstractValidator<TransferFundsCommand>
{
    public TransferFundsValidator()
    {
        RuleFor(r => r.FromWalletId)
           .NotEmpty().WithMessage("{PropertyName} should have value.");

        RuleFor(r => r.ToWalletId)
           .NotEmpty().WithMessage("{PropertyName} should have value.");

        RuleFor(r => r.Amount)
           .NotEmpty().WithMessage("{PropertyName} should have value.")
           .GreaterThan(0).WithMessage("{PropertyName} should be greater than 0");

    }
}
