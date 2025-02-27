using MediatR;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Commands.DeleteUserCreatedSagaInstance;

public sealed class DeleteUserCreatedSagaInstanceCommand : IRequest<DeleteUserCreatedSagaInstanceResponse>
{
    public Guid CorrelationId { get; set; }
}
