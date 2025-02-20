using MediatR;

namespace SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Features.Queries;

public sealed class GetUserCreatedSagOrchestratorInstanceQuery 
    : IRequest<GetUserCreatedSagOrchestratorInstanceResponse>
{
    public Guid CorrelationId { get; set; }
}
