using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Common.Specifications;

public sealed class GetUserCreatedSagaOrchestratorInstanceByCorrelationId(Guid correlationId)
    : BaseSpecification<UserCreatedSagaStateInstance>(x => x.CorrelationId == correlationId)
{
}
