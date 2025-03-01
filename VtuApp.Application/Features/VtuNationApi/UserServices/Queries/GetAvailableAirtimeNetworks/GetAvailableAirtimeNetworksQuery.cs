using SharedKernel.Application.Interfaces;
using VtuApp.Application.HelperClasses;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.GetAvailableAirtimeNetworks;

public sealed class GetAvailableAirtimeNetworksQuery : ICachedQuery<GetAvailableAirtimeNetworksResponse>
{
    public string CacheKey => CacheHelperVtuApp.GenerateGetAvailableAirtimeNetworksQueryCacheKey();

    public TimeSpan? Expiration => null;
}
