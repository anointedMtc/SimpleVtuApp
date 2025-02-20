using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.AuthenticateUser;

public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserValidator()
    {
        RuleFor(x => x.UserForAuthentication.Email)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

        RuleFor(x => x.UserForAuthentication.Password)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");


    }
}
