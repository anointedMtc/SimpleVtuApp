using FluentValidation;

namespace Wallet.Application.Features.Queries.GetWalletById;

public class GetWalletByIdValidator : AbstractValidator<GetWalletByIdQuery>
{
    public GetWalletByIdValidator()
    {

        RuleFor(r => r.WalletId)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
