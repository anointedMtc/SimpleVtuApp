using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Application.HelperClasses;

public static class CacheHelperSagas
{
    public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
    private static readonly string _getAllUserCreatedSagaKeyTemplate = "allUserCreatedSaga-{0}-{1}-{2}-{3}";
    private static readonly string _getAllVtuAirtimeSagaKeyTemplate = "allVtuAirtimeSaga-{0}-{1}-{2}-{3}";
    private static readonly string _getAllVtuDataSagaKeyTemplate = "allVtuDataSaga-{0}-{1}-{2}-{3}";
    private static readonly string _getUserCreatedSagaSingleInstanceKeyTemplate = "userCreatedSagaSingleInstance-{0}-{1}-{2}-{3}";
    private static readonly string _getVtuAirtimeSagaSingleInstanceKeyTemplate = "vtuAirtimeSagaSingleInstance-{0}-{1}-{2}-{3}";
    private static readonly string _getVtuDataSagaSingleInstanceKeyTemplate = "vtuAirtimeSagaSingleInstance-{0}-{1}-{2}-{3}";



    public static string GenerateGetAllUserCreatedSagaCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllUserCreatedSagaKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber , paginationFilter.PageSize);
    }

    public static string GenerateGetAllVtuAirtimeSagaCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllVtuAirtimeSagaKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber, paginationFilter.PageSize);
    }

    public static string GenerateGetAllVtuDataSagaCacheKey(PaginationFilter paginationFilter)
    {
        return string.Format(_getAllVtuDataSagaKeyTemplate, paginationFilter.Search, paginationFilter.Sort, paginationFilter.PageNumber, paginationFilter.PageSize);
    }




    public static string GenerateGetUserCreatedSagaSingleInstanceCacheKey(Guid id)
    {
        return string.Format(_getUserCreatedSagaSingleInstanceKeyTemplate, " ", " ", " ", id);
    }

    public static string GenerateGetVtuAirtimeSagaSingleInstanceCacheKey(Guid id)
    {
        return string.Format(_getVtuAirtimeSagaSingleInstanceKeyTemplate, " ", " ", " ", id);
    }

    public static string GenerateGetVtuDataSagaSingleInstanceCacheKey(Guid id)
    {
        return string.Format(_getVtuDataSagaSingleInstanceKeyTemplate, " ", " ", " ", id);
    }








}
