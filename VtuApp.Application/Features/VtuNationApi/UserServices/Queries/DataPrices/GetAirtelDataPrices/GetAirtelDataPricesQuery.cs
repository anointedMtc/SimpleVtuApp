using SharedKernel.Application.Interfaces;
using VtuApp.Application.HelperClasses;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetAirtelDataPrices;

public class GetAirtelDataPricesQuery : ICachedQuery<GetAirtelDataPricesResponse>
{
    public string CacheKey => CacheHelperVtuApp.GenerateGetAirtelDataPricesQueryCacheKey();

    public TimeSpan? Expiration => null;
}
