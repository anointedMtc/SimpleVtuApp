using MediatR;

namespace SagaOrchestrationStateMachines.VtuDataOrderedSagaOrchestrator.Helpers.Features.Queries;

public sealed class GetVtuDataOrderedSagaStateInstanceQuery
    : IRequest<GetVtuDataOrderedSagaStateInstanceResponse>
{
    public Guid CorrelationId { get; set; }
}
