using SharedKernel.Domain.HelperClasses;

namespace VtuApp.Application.HelperClasses;

public static class CacheHelperVtuApp
{
    public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
    private static readonly string _getAllVtuCustomersKeyTemplate = "customers-{0}-{1}-{2}-{3}";



    public static string GenerateGetAllVtuCustomersCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllVtuCustomersKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber, paginationFilter.PageSize);
    }

  
}
