using FluentValidation;

namespace Identity.Application.Features.RoleManagement.Commands.UpdateApplicationRole;

public class UpdateApplicationRoleValidator : AbstractValidator<UpdateApplicationRoleCommand>
{
    public UpdateApplicationRoleValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

        RuleFor(e => e.UpdateApplicationRoleRequestDto.Name)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");


        RuleFor(e => e.UpdateApplicationRoleRequestDto.Description)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

    }
}
