using SagaOrchestrationStateMachines.Infrastructure.VtuDataOrderedSagaOrchestrator;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Domain.Specifications.VtuDataSaga;

public sealed class GetVtuDataOrderedSagaOrchestratorInstanceByCorrelationId(Guid correlationId)
    : BaseSpecification<VtuDataOrderedSagaStateInstance>(x => x.CorrelationId == correlationId)
{
}
