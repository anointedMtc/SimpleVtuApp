using FluentValidation;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.UsersEndpoints.RegisterUser;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    private string[] allowedGender = ["Male", "Female"];    // don't do this... people identify as more than binary male and female... this could be a huge disaster... but i could have used it to check and make sure that people only enter male and female in the options provided... 

    public RegisterUserValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;

        RuleFor(r => r.UserForRegisteration.FirstName)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .Length(4, 12).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength}. {PropertyValue} is {TotalLength} characters and does not meet requirements")
            .Must(IsValidName).WithMessage("{PropertyName} should all be letters. {PropertyValue} does not meet requirements");

        RuleFor(r => r.UserForRegisteration.LastName)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .Length(4, 12).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength}. {PropertyValue} is {TotalLength} characters and does not meet requirements")
            .Must(IsValidLetterOrDigit).WithMessage("{PropertyName} can only be a letter or decimal digit. {PropertyValue} does not meet requirements");

        RuleFor(r => r.UserForRegisteration.Email)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

        RuleFor(r => r.UserForRegisteration.Password)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .MinimumLength(7).WithMessage("{PropertyName} must be at least 7 characters long. {PropertyValue} does not meet requirements")
            .NotEqual("password").WithMessage("{PropertyName} cannot be equal to {ComparisonValue}. {PropertyValue} does not meet requirements")
            .NotEqual(r => r.UserForRegisteration.FirstName).WithMessage("{PropertyName} cannot be equal to your {ComparisonProperty}. {PropertyValue} does not meet requirements.")
            .NotEqual(r => r.UserForRegisteration.LastName).WithMessage("{PropertyName} cannot be equal to your {ComparisonProperty}. {PropertyValue} does not meet requirements.");

        RuleFor(r => r.UserForRegisteration.ConfirmPassword)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .Equal(r => r.UserForRegisteration.Password).WithMessage("Passwords do not match!")
            .When(r => !string.IsNullOrWhiteSpace(r.UserForRegisteration.Password));

        RuleFor(r => r.UserForRegisteration.Gender)
            .NotEmpty().WithMessage("{PropertyName} should have value");

        RuleFor(r => r.UserForRegisteration.Nationality)
            .NotEmpty()
            .NotNull()
            .WithMessage("{PropertyName} should have value");

        RuleFor(r => r.UserForRegisteration.IsTwoFacAuthEnabled)
            .NotEmpty()
            .NotNull()
            .WithMessage("{PropertyName} should have value");

    }

    
    private bool IsValidName(string name)
    {
        return name.All(char.IsLetter);

    }

    private bool IsValidLetterOrDigit(string name)
    {
        char[] chars = name.ToCharArray();

        return chars.All(char.IsLetterOrDigit);
    }

    // this check is not necessary as the userManager class does this automatically for us... except you want to check for something else.. it makes sure that Email and UserName are unique and not already taken by another user
    // check to know if the userName has already been taken by another person
    private async Task<bool> IsUniqueUserName(string? userName, CancellationToken cancellationToken)
    {
        //var uniqueUserName = _userManager.Users.Any(u => u.UserName == userName);

        if (_userManager.Users.Any(u => u.UserName == userName))
        {
            return false;
        }

        return true;
    }

}
