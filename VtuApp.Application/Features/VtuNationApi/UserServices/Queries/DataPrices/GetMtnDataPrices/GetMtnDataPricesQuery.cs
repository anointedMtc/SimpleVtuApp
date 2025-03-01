using SharedKernel.Application.Interfaces;
using VtuApp.Application.HelperClasses;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetMtnDataPrices;

public class GetMtnDataPricesQuery : ICachedQuery<GetMtnDataPricesResponse>
{
    public string CacheKey => CacheHelperVtuApp.GenerateGetMtnDataPricesQueryCacheKey();

    public TimeSpan? Expiration => null;
}
