using MediatR;
using SharedKernel.Application.Interfaces;
using Wallet.Application.HelperClasses;

namespace Wallet.Application.Features.Queries.GetWalletById;

public class GetWalletByIdQuery : ICachedQuery<GetWalletByIdResponse>
{
    public Guid WalletId { get; set; }

    public string CacheKey => CacheHelperWallet.GenerateGetWalletByIdCacheKey(WalletId);

    public TimeSpan? Expiration => null;
}
