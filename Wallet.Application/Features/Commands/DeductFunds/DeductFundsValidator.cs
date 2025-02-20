using FluentValidation;

namespace Wallet.Application.Features.Commands.DeductFunds;

public class DeductFundsValidator : AbstractValidator<DeductFundsCommand>
{
    public DeductFundsValidator()
    {
        RuleFor(r => r.WalletId)
            .NotEmpty().WithMessage("{PropertyName} should have value.");

        RuleFor(r => r.Amount)
           .NotEmpty().WithMessage("{PropertyName} should have value.")
           .GreaterThan(0).WithMessage("{PropertyName} should be greater than 0");

    }
}
