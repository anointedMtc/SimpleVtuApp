using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdateProfileVtuNation;

internal sealed class UpdateProfileVtuNationValidator : AbstractValidator<UpdateProfileVtuNationCommand>
{
    public UpdateProfileVtuNationValidator()
    {
        RuleFor(r => r.UpdateProfileRequestVtuNation.FirstName)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.UpdateProfileRequestVtuNation.LastName)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
