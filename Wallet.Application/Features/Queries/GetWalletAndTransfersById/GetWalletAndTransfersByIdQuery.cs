using SharedKernel.Application.Interfaces;
using Wallet.Application.HelperClasses;

namespace Wallet.Application.Features.Queries.GetWalletAndTransfersById;

public sealed class GetWalletAndTransfersByIdQuery : ICachedQuery<GetWalletAndTransfersByIdResponse>
{
    public Guid WalletId { get; set; }

    public string CacheKey => CacheHelperWallet.GenerateGetWalletAndTransfersByIdCacheKey(WalletId);

    public TimeSpan? Expiration => null;
}
