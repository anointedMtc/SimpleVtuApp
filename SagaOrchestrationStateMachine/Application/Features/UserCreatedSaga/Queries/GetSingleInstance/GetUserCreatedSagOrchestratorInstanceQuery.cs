using SagaOrchestrationStateMachines.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetSingleInstance;

public sealed class GetUserCreatedSagOrchestratorInstanceQuery
    : ICachedQuery<GetUserCreatedSagOrchestratorInstanceResponse>
{
    public Guid CorrelationId { get; set; }

    public string CacheKey => CacheHelperSagas.GenerateGetUserCreatedSagaSingleInstanceCacheKey(CorrelationId);

    public TimeSpan? Expiration => null;
}
