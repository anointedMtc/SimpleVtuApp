using MediatR;

namespace SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetSingleInstance;

public sealed class GetVtuDataOrderedSagaStateInstanceQuery
    : IRequest<GetVtuDataOrderedSagaStateInstanceResponse>
{
    public Guid CorrelationId { get; set; }
}
