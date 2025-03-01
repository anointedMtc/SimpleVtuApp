using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.HelperClasses;
using Wallet.Application.HelperClasses;

namespace Wallet.Application.Features.Queries.GetAllWallets;

public sealed class GetAllWalletsQuery : ICachedQuery<Pagination<GetAllWalletsResponse>>
{
    public GetAllWalletsQuery(PaginationFilter paginationFilter) : base()
    {
        PaginationFilter = paginationFilter;
    }

    public PaginationFilter PaginationFilter { get; set; }

    public string CacheKey => CacheHelperWallet.GenerateGetAllWalletsCacheKey(PaginationFilter);

    public TimeSpan? Expiration => null;

}