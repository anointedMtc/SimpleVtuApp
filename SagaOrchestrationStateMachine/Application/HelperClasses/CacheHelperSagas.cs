using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Application.HelperClasses;

public static class CacheHelperSagas
{
    public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
    private static readonly string _getAllUserCreatedSagaKeyTemplate = "roles-{0}-{1}-{2}-{3}";
    private static readonly string _getAllVtuAirtimeSagaKeyTemplate = "roles-{0}-{1}-{2}-{3}";
    private static readonly string _getAllVtuDataSagaKeyTemplate = "roles-{0}-{1}-{2}-{3}";



    public static string GenerateGetAllUserCreatedSagaCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllUserCreatedSagaKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber, paginationFilter.PageSize);
    }

    public static string GenerateGetAllVtuAirtimeSagaCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllVtuAirtimeSagaKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber, paginationFilter.PageSize);
    }

    public static string GenerateGetAllVtuDataSagaCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllVtuDataSagaKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber, paginationFilter.PageSize);
    }


}
