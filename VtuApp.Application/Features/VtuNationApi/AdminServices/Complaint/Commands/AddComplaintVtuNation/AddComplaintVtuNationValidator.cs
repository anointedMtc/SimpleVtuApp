using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Complaint.Commands.AddComplaintVtuNation;

internal sealed class AddComplaintVtuNationValidator : AbstractValidator<AddComplaintVtuNationCommand>
{
    public AddComplaintVtuNationValidator()
    {
        RuleFor(r => r.AddComplaintRequestVtuNation.Subject)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.AddComplaintRequestVtuNation.Message)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.AddComplaintRequestVtuNation.Channel)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.AddComplaintRequestVtuNation.ComplaintCategory)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
