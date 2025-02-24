using FluentValidation;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetSingleInstance;

public sealed class GetUserCreatedSagOrchestratorInstanceValidator
    : AbstractValidator<GetUserCreatedSagOrchestratorInstanceQuery>
{
    public GetUserCreatedSagOrchestratorInstanceValidator()
    {
        RuleFor(r => r.CorrelationId)
          .NotEmpty().WithMessage("{PropertyName} should have value.");

    }
}
