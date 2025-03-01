using FluentValidation;

namespace Wallet.Application.Features.Queries.GetOwnerAndWalletByEmail;

internal sealed class GetOwnerAndWalletByEmailValidator 
    : AbstractValidator<GetOwnerAndWalletByEmailQuery>
{
    public GetOwnerAndWalletByEmailValidator()
    {
        RuleFor(r => r.Email)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
