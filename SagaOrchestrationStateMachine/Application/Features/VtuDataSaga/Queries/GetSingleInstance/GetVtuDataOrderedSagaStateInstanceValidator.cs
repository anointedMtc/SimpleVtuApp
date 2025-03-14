﻿using FluentValidation;

namespace SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetSingleInstance;

public sealed class GetVtuDataOrderedSagaStateInstanceValidator
    : AbstractValidator<GetVtuDataOrderedSagaStateInstanceQuery>
{
    public GetVtuDataOrderedSagaStateInstanceValidator()
    {
        RuleFor(r => r.CorrelationId)
        .NotEmpty().WithMessage("{PropertyName} should have value.");

    }
}
