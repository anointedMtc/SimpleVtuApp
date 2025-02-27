using FluentValidation;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Commands.DeleteUserCreatedSagaInstance;

internal sealed class DeleteUserCreatedSagaInstanceValidator : AbstractValidator<DeleteUserCreatedSagaInstanceCommand>
{
    public DeleteUserCreatedSagaInstanceValidator()
    {
        RuleFor(e => e.CorrelationId)
            .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
