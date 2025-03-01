using SharedKernel.Application.Interfaces;
using VtuApp.Application.HelperClasses;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.Get9MobileDataPrices;

public class Get9MobileDataPricesQuery : ICachedQuery<Get9MobileDataPricesResponse>
{
    public string CacheKey => CacheHelperVtuApp.GenerateGet9MobileDataPricesQueryCacheKey();

    public TimeSpan? Expiration => null;
}
