using FluentValidation;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetAllSagaInstance;

public sealed class GetAllUserCreatedSagaInstanceValidator : AbstractValidator<GetAllUserCreatedSagaInstanceQuery>
{
    public GetAllUserCreatedSagaInstanceValidator()
    {

    }
}
