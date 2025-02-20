using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Commands.SubmitPaymentNotificationVtuNation;

internal sealed class SubmitPaymentNotificationVtuNationValidator : AbstractValidator<SubmitPaymentNotificationVtuNationCommand>
{
    public SubmitPaymentNotificationVtuNationValidator()
    {
        RuleFor(r => r.SubmitPaymentNotificationRequestVtuNation.Description)
         .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.SubmitPaymentNotificationRequestVtuNation.Amount)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements")
           .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonProperty}.");

        RuleFor(r => r.SubmitPaymentNotificationRequestVtuNation.Ref)
         .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
