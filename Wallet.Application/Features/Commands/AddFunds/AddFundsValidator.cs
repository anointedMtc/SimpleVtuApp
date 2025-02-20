using FluentValidation;

namespace Wallet.Application.Features.Commands.AddFunds;

public class AddFundsValidator : AbstractValidator<AddFundsCommand>
{
    public AddFundsValidator()
    {
        RuleFor(r => r.WalletId)
            .NotEmpty().WithMessage("{PropertyName} should have value.");

        RuleFor(r => r.Amount)
           .NotEmpty().WithMessage("{PropertyName} should have value.")
           .GreaterThan(0).WithMessage("{PropertyName} should be greater than 0");

    }
}
