using SagaOrchestrationStateMachines.Application.HelperClasses;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetAllSagaInstance;

public sealed class GetAllVtuDataSagaInstanceQuery : ICachedQuery<Pagination<GetAllVtuDataSagaInstanceResponse>>
{
    public GetAllVtuDataSagaInstanceQuery(PaginationFilter paginationFilter) : base()
    {
        PaginationFilter = paginationFilter;
    }

    public PaginationFilter PaginationFilter { get; set; }

    public string CacheKey => CacheHelperSagas.GenerateGetAllVtuDataSagaCacheKey(PaginationFilter);

    public TimeSpan? Expiration => null;

}
