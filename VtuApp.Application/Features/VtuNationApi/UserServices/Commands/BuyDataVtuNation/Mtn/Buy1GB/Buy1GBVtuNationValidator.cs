using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy1GB;

internal sealed class Buy1GBVtuNationValidator : AbstractValidator<Buy1GBVtuNationCommand>
{
    public Buy1GBVtuNationValidator()
    {
        RuleFor(r => r.PhoneNumber)
        .NotEmpty().WithMessage("{PropertyName} should have value.")
        .MinimumLength(11).WithMessage("{PropertyName} should me minimum of {ComparisonValue}. {PropertyValue} does not meet requirement.");

    }
}
