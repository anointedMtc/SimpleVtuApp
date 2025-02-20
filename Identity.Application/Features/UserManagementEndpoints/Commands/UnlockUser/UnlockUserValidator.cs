using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UnlockUser;

public class UnlockUserValidator : AbstractValidator<UnlockUserCommand>
{
    public UnlockUserValidator()
    {
        RuleFor(r => r.UserId)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
