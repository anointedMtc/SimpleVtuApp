using FluentValidation;

namespace Identity.Application.Features.RoleManagement.Commands.AddApplicationRole;

public class AddApplicationRoleValidator : AbstractValidator<AddApplicationRoleCommand>
{
    public AddApplicationRoleValidator()
    {
        RuleFor(e => e.AddApplicationRoleRequestDto.Name)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

        // No need since it is automatically set
        //RuleFor(e => e.AddApplicationRoleRequestDto.NormalizedName)
        //    .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

        RuleFor(e => e.AddApplicationRoleRequestDto.Description)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

    }
}
