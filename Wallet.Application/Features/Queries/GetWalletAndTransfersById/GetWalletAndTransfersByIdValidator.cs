using FluentValidation;

namespace Wallet.Application.Features.Queries.GetWalletAndTransfersById;

internal sealed class GetWalletAndTransfersByIdValidator 
    : AbstractValidator<GetWalletAndTransfersByIdQuery>
{
    public GetWalletAndTransfersByIdValidator()
    {
        RuleFor(r => r.WalletId)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
