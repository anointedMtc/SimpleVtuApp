using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SubmitBvnVtuNation;

internal sealed class SubmitBvnVtuNationValidator : AbstractValidator<SubmitBvnVtuNationCommand>
{
    public SubmitBvnVtuNationValidator()
    {

        RuleFor(r => r.SubmitBvnRequestVtuNation.Bvn)
           .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
