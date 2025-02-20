using FluentValidation;
using VtuApp.Shared.Constants;
using VtuApp.Shared.DTO.VtuNationApi.Constants;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation;

internal sealed class BuyDataVtuNationValidator : AbstractValidator<BuyDataVtuNationCommand>
{
    // how to do it as list - we also have how to do it as Enum... 
    private readonly List<string> validNetworkCategories = [NetworkProvider.Mtn.ToString(), NetworkProvider.Airtel.ToString(), NetworkProvider.Glo.ToString(), NetworkProvider.NineMobile.ToString()];
    private readonly List<string> validDataPlans = [VtuNationDataConstants.MtnFiveHundredMBName, VtuNationDataConstants.MtnOneGBName, VtuNationDataConstants.MtnTwoGBName, VtuNationDataConstants.MtnThreeGBName, VtuNationDataConstants.MtnFiveGBName, VtuNationDataConstants.MtnTenGBName];

    //private readonly List<string> validNetworkCategories = ["Mtn", "Airtel", "Glo", "9Mobile"];
    //private readonly List<string> validDataPlans = ["500MB", "1GB", "2GB", "3GB", "5GB", "10GB"];


    public BuyDataVtuNationValidator()
    {
        RuleFor(r => r.BuyDataRequestVtuNation.DataPlan)
           .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
           .Must(validDataPlans.Contains).WithMessage("{PropertyValue} is an Invalid category. Please choose from the list of valid categories {ComparisonValue}.");
        

        RuleFor(r => r.BuyDataRequestVtuNation.Network)
               .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
               .Must(validNetworkCategories.Contains).WithMessage("{PropertyValue} is an Invalid category. Please choose from the list of valid categories {ComparisonValue}.");

        RuleFor(r => r.BuyDataRequestVtuNation.MobileNumber)
               .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
               .MinimumLength(11).WithMessage("{PropertyName} should me minimum of {ComparisonValue}. {PropertyValue} does not meet requirements.");
        

    }
}
