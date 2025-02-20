using FluentValidation;

namespace Identity.Application.Features.Utilities.TriggerUserCreatedEventSaga;

internal class TriggerUserCreatedEventSagaValidator : AbstractValidator<TriggerUserCreatedEventSagaCommand>
{
    public TriggerUserCreatedEventSagaValidator()
    {
        RuleFor(e => e.UserEmail)
            .NotEmpty().WithMessage("{PropertyName} should have value.")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

    }
}
