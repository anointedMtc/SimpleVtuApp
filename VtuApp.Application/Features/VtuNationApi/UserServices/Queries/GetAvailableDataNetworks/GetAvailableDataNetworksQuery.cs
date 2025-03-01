using SharedKernel.Application.Interfaces;
using VtuApp.Application.HelperClasses;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.GetAvailableDataNetworks;

public sealed class GetAvailableDataNetworksQuery : ICachedQuery<GetAvailableDataNetworksResponse>
{
    public string CacheKey => CacheHelperVtuApp.GenerateGetAvailableDataNetworksQueryCacheKey();

    public TimeSpan? Expiration => null;
}
