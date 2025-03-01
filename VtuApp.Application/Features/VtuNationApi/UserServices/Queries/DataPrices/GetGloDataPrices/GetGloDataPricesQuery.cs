using MediatR;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.HelperClasses;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetGloDataPrices;

public class GetGloDataPricesQuery : ICachedQuery<GetGloDataPricesResponse>
{
    public string CacheKey => CacheHelperVtuApp.GenerateGetGloDataPricesQueryCacheKey();

    public TimeSpan? Expiration => null;
}
