using FluentValidation;
using VtuApp.Shared.Constants;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyAirtimeVtuNation;

public sealed class BuyAirtimeVtuNationValidator : AbstractValidator<BuyAirtimeVtuNationCommand>
{
    private readonly List<string> validNetworkCategories = [NetworkProvider.Mtn.ToString(), NetworkProvider.Airtel.ToString(), NetworkProvider.Glo.ToString(), NetworkProvider.NineMobile.ToString()];

    //private readonly List<string> validNetworkCategories = ["Mtn", "Airtel", "Glo", "9Mobile"];

    public BuyAirtimeVtuNationValidator()
    {
        RuleFor(r => r.BuyAirtimeRequestVtuNation.Amount)
           .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
           .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonProperty}.");

        RuleFor(r => r.BuyAirtimeRequestVtuNation.Network)
           .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
           .Must(validNetworkCategories.Contains).WithMessage("{PropertyValue} is an Invalid category. Please choose from the list of valid categories {ComparisonValue}.");

        RuleFor(r => r.BuyAirtimeRequestVtuNation.MobileNumber)
          .NotEmpty().WithMessage("{PropertyName} should have value.")
          .MinimumLength(11).WithMessage("{PropertyName} should me minimum of {ComparisonValue}. {PropertyValue} does not meet requirement.");

    }
}
