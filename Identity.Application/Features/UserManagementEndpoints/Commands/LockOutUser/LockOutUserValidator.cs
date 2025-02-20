using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.LockOutUser;

public class LockOutUserValidator : AbstractValidator<LockOutUserCommand>
{
    public LockOutUserValidator()
    {
        RuleFor(r => r.UserId)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
