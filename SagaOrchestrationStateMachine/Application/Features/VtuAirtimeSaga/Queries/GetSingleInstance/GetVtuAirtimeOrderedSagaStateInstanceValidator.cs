using FluentValidation;

namespace SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetSingleInstance;

public sealed class GetVtuAirtimeOrderedSagaStateInstanceValidator
    : AbstractValidator<GetVtuAirtimeOrderedSagaStateInstanceQuery>
{
    public GetVtuAirtimeOrderedSagaStateInstanceValidator()
    {
        RuleFor(r => r.CorrelationId)
         .NotEmpty().WithMessage("{PropertyName} should have value.");

    }
}
