using MediatR;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetSingleInstance;

public sealed class GetUserCreatedSagOrchestratorInstanceQuery
    : IRequest<GetUserCreatedSagOrchestratorInstanceResponse>
{
    public Guid CorrelationId { get; set; }
}
