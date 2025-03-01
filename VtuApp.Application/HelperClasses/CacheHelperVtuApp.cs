using SharedKernel.Domain.HelperClasses;

namespace VtuApp.Application.HelperClasses;

public static class CacheHelperVtuApp
{
    public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
    private static readonly string _getAllVtuCustomersKeyTemplate = "customers-{0}-{1}-{2}-{3}";
    private static readonly string _getCustomerAndBonusTransfersAndVtuTransactionsQueryKeyTemplate = "customerAndBonusTransfersAndVtuTransactions-{0}-{1}-{2}-{3}";

    // VTU-NATION
    private static readonly string _getAvailableAirtimeNetworksQueryKeyTemplate = "availableAirtimeNetworks-{0}-{1}-{2}-{3}";
    private static readonly string _getAvailableDataNetworksQueryKeyTemplate = "availableDataNetworks-{0}-{1}-{2}-{3}";
    private static readonly string _get9MobileDataPricesQueryKeyTemplate = "9MobileDataPrices-{0}-{1}-{2}-{3}";
    private static readonly string _getAirtelDataPricesQueryKeyTemplate = "airtelDataPrices-{0}-{1}-{2}-{3}";
    private static readonly string _getGloDataPricesQueryKeyTemplate = "gloDataPrices-{0}-{1}-{2}-{3}";
    private static readonly string _getMtnDataPricesQueryKeyTemplate = "mtnDataPrices-{0}-{1}-{2}-{3}";


    public static string GenerateGetAllVtuCustomersCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllVtuCustomersKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber, paginationFilter.PageSize);
    }

    public static string GenerateGetCustomerAndBonusTransfersAndVtuTransactionsQueryCacheKey(string email)
    {
        return string.Format(_getCustomerAndBonusTransfersAndVtuTransactionsQueryKeyTemplate, " ", " ", " ", email);
    }



    // VTU-NATION

    public static string GenerateGetAvailableAirtimeNetworksQueryCacheKey()
    {
        return string.Format(_getAvailableAirtimeNetworksQueryKeyTemplate, " ", " ", " ", " ");
    }
    public static string GenerateGetAvailableDataNetworksQueryCacheKey()
    {
        return string.Format(_getAvailableDataNetworksQueryKeyTemplate, " ", " ", " ", " ");
    }

    public static string GenerateGet9MobileDataPricesQueryCacheKey()
    {
        return string.Format(_get9MobileDataPricesQueryKeyTemplate, " ", " ", " ", " ");
    }

    public static string GenerateGetAirtelDataPricesQueryCacheKey()
    {
        return string.Format(_getAirtelDataPricesQueryKeyTemplate, " ", " ", " ", " ");
    }

    public static string GenerateGetGloDataPricesQueryCacheKey()
    {
        return string.Format(_getGloDataPricesQueryKeyTemplate, " ", " ", " ", " ");
    }

    public static string GenerateGetMtnDataPricesQueryCacheKey()
    {
        return string.Format(_getMtnDataPricesQueryKeyTemplate, " ", " ", " ", " ");
    }











}
