using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.RegisterWithVtuNationApi;

internal sealed class RegisterWithVtuNationApiValidator : AbstractValidator<RegisterWithVtuNationApiCommand>
{
    public RegisterWithVtuNationApiValidator()
    {
        RuleFor(r => r.RegisterRequestVtuNation.FirstName)
          .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.RegisterRequestVtuNation.LastName)
         .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.RegisterRequestVtuNation.Password)
        .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

        RuleFor(r => r.RegisterRequestVtuNation.Email)
           .NotEmpty().WithMessage("{PropertyName} should have value.")
           .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

        RuleFor(r => r.RegisterRequestVtuNation.PhoneNumber)
           .NotEmpty().WithMessage("{PropertyName} should have value.")
           .MinimumLength(11).WithMessage("{PropertyName} should me minimum of {ComparisonValue}. {PropertyValue} does not meet requirement.");

    }
}
