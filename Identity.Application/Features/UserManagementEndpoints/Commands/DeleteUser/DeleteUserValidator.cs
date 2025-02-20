using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.DeleteUser;

public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(r => r.UserId)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
