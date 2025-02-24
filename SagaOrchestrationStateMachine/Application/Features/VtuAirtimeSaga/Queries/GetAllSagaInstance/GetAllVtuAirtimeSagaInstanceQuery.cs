using ApplicationSharedKernel.HelperClasses;
using ApplicationSharedKernel.Interfaces;
using SagaOrchestrationStateMachines.Application.HelperClasses;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetAllSagaInstance;

public sealed class GetAllVtuAirtimeSagaInstanceQuery : ICachedQuery<Pagination<GetAllVtuAirtimeSagaInstanceResponse>>
{
    public GetAllVtuAirtimeSagaInstanceQuery(PaginationFilter paginationFilter) : base()
    {
        PaginationFilter = paginationFilter;
    }

    public PaginationFilter PaginationFilter { get; set; }

    public string CacheKey => CacheHelperSagas.GenerateGetAllVtuAirtimeSagaCacheKey(PaginationFilter);

    public TimeSpan? Expiration => null;

}
