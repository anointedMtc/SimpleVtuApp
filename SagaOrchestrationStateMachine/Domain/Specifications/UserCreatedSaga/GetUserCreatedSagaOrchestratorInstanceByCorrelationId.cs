using SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Domain.Specifications.UserCreatedSaga;

public sealed class GetUserCreatedSagaOrchestratorInstanceByCorrelationId(Guid correlationId)
    : BaseSpecification<UserCreatedSagaStateInstance>(x => x.CorrelationId == correlationId)
{
}
