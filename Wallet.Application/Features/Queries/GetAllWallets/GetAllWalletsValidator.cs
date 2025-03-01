using FluentValidation;

namespace Wallet.Application.Features.Queries.GetAllWallets;

internal sealed class GetAllWalletsValidator : AbstractValidator<GetAllWalletsQuery>
{
    public GetAllWalletsValidator()
    {
        
    }
}
