using MediatR;

namespace Identity.Application.Features.Utilities.TriggerUserCreatedEventSaga;

public sealed class TriggerUserCreatedEventSagaCommand : IRequest<TriggerUserCreatedEventSagaResponse>
{
    public string UserEmail { get; set; }
}
