using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserByEmail;

public class GetUserByEmailValidator : AbstractValidator<GetUserByEmailQuery>
{
    public GetUserByEmailValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

    }
}
