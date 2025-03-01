using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.HelperClasses;
using Wallet.Application.HelperClasses;

namespace Wallet.Application.Features.Queries.GetAllOwners;

public sealed class GetAllOwnersQuery : ICachedQuery<Pagination<GetAllOwnersResponse>>
{
    public GetAllOwnersQuery(PaginationFilter paginationFilter) : base()
    {
        PaginationFilter = paginationFilter;
    }

    public PaginationFilter PaginationFilter { get; set; }

    public string CacheKey => CacheHelperWallet.GenerateGetAllOwnersCacheKey(PaginationFilter);

    public TimeSpan? Expiration => null;

}