using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy10GB;

internal sealed class Buy10GBVtuNationValidator : AbstractValidator<Buy10GBVtuNationCommand>
{
    public Buy10GBVtuNationValidator()
    {
        RuleFor(r => r.PhoneNumber)
        .NotEmpty().WithMessage("{PropertyName} should have value.")
        .MinimumLength(11).WithMessage("{PropertyName} should me minimum of {ComparisonValue}. {PropertyValue} does not meet requirement.");

    }
}
