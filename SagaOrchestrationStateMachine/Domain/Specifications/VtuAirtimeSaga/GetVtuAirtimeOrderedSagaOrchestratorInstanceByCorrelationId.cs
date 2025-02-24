using SagaOrchestrationStateMachines.Infrastructure.VtuAirtimeOrderedSagaOrchestrator;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Domain.Specifications.VtuAirtimeSaga;

public sealed class GetVtuAirtimeOrderedSagaOrchestratorInstanceByCorrelationId(Guid correlationId)
    : BaseSpecification<VtuAirtimeOrderedSagaStateInstance>(x => x.CorrelationId == correlationId)
{
}
