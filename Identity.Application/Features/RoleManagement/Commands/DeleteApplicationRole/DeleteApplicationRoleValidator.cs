using FluentValidation;

namespace Identity.Application.Features.RoleManagement.Commands.DeleteApplicationRole;

public class DeleteApplicationRoleValidator : AbstractValidator<DeleteApplicationRoleCommand>
{
    public DeleteApplicationRoleValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

    }
}
