using FluentValidation;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.UsersEndpoints.UpdateUserDetails;

public class UpdateUserDetailsValidator : AbstractValidator<UpdateUserDetailsCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateUserDetailsValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;


        RuleFor(r => r.UpdateUserRequestDto.FirstName)
           .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
           .Length(4, 12).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength}. {PropertyValue} is {TotalLength} characters and does not meet requirements")
           .Must(IsValidName).WithMessage("{PropertyName} should all be letters. {PropertyValue} does not meet requirements");

        RuleFor(r => r.UpdateUserRequestDto.LastName)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .Length(4, 12).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength}. {PropertyValue} is {TotalLength} characters and does not meet requirements")
            .Must(IsValidLetterOrDigit).WithMessage("{PropertyName} can only be a letter or decimal digit. {PropertyValue} does not meet requirements");

        //RuleFor(r => r.UpdateUserRequestDto.Email)
        //    .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
        //    .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

        RuleFor(r => r.UpdateUserRequestDto.UserName)
           .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
           .Length(4, 12).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength}. {PropertyValue} is {TotalLength} characters and does not meet requirements")
           .MustAsync(IsUniqueUserName).WithMessage("{PropertyValue} is already taken by another user, please try a different UserName");

        RuleFor(r => r.UpdateUserRequestDto.Gender)
            .NotEmpty().WithMessage("{PropertyName} should have value");

        RuleFor(r => r.UpdateUserRequestDto.DateOfBirth)
            .NotEmpty().WithMessage("{PropertyName} should have value");

        RuleFor(r => r.UpdateUserRequestDto.Nationality)
            .NotEmpty().WithMessage("{PropertyName} should have value");


        //RuleFor(r => r.UpdateUserRequestDto.IsTwoFacAuthEnabled)
        //    .NotEmpty().WithMessage("{PropertyName} should have value");
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
