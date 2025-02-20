using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy2GB;

internal sealed class Buy2GBVtuNationValidator : AbstractValidator<Buy2GBVtuNationCommand>
{
    public Buy2GBVtuNationValidator()
    {
        RuleFor(r => r.PhoneNumber)
         .NotEmpty().WithMessage("{PropertyName} should have value.")
         .MinimumLength(11).WithMessage("{PropertyName} should me minimum of {ComparisonValue}. {PropertyValue} does not meet requirement.");

    }
}
