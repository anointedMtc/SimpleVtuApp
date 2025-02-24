using MediatR;

namespace SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetSingleInstance;

public sealed class GetVtuAirtimeOrderedSagaStateInstanceQuery
    : IRequest<GetVtuAirtimeOrderedSagaStateInstanceResponse>
{
    public Guid CorrelationId { get; set; }
}
