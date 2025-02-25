using SagaOrchestrationStateMachines.Application.HelperClasses;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetAllSagaInstance;

public sealed class GetAllUserCreatedSagaInstanceQuery : ICachedQuery<Pagination<GetAllUserCreatedSagaInstanceResponse>>
{
    public GetAllUserCreatedSagaInstanceQuery(PaginationFilter paginationFilter) : base()
    {
        PaginationFilter = paginationFilter;
    }

    public PaginationFilter PaginationFilter { get; set; }

    public string CacheKey => CacheHelperSagas.GenerateGetAllUserCreatedSagaCacheKey(PaginationFilter);

    public TimeSpan? Expiration => null;

}
