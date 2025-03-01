using SagaOrchestrationStateMachines.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetSingleInstance;

public sealed class GetVtuDataOrderedSagaStateInstanceQuery
    : ICachedQuery<GetVtuDataOrderedSagaStateInstanceResponse>
{
    public Guid CorrelationId { get; set; }

    public string CacheKey => CacheHelperSagas.GenerateGetVtuDataSagaSingleInstanceCacheKey(CorrelationId);

    public TimeSpan? Expiration => null;
}
