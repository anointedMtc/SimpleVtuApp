using SagaOrchestrationStateMachines.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetSingleInstance;

public sealed class GetVtuAirtimeOrderedSagaStateInstanceQuery
    : ICachedQuery<GetVtuAirtimeOrderedSagaStateInstanceResponse>
{
    public Guid CorrelationId { get; set; }

    public string CacheKey => CacheHelperSagas.GenerateGetVtuAirtimeSagaSingleInstanceCacheKey(CorrelationId);

    public TimeSpan? Expiration => null;
}
