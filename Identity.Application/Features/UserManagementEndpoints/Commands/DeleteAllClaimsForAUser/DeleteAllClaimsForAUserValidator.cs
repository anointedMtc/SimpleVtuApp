using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.DeleteAllClaimsForAUser;

public class DeleteAllClaimsForAUserValidator : AbstractValidator<DeleteAllClaimsForAUserCommand>
{
    public DeleteAllClaimsForAUserValidator()
    {
        RuleFor(r => r.UserId)
         .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
