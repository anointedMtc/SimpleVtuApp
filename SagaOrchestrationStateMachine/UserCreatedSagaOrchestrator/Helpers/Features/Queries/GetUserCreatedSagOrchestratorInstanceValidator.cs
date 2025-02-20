using FluentValidation;

namespace SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Features.Queries;

public sealed class GetUserCreatedSagOrchestratorInstanceValidator 
    : AbstractValidator<GetUserCreatedSagOrchestratorInstanceQuery>
{
    public GetUserCreatedSagOrchestratorInstanceValidator()
    {
        RuleFor(r => r.CorrelationId)
          .NotEmpty().WithMessage("{PropertyName} should have value.");

    }
}
