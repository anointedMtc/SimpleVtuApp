using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UnAssignUserRole;

public class UnassignUserRoleValidator : AbstractValidator<UnassignUserRoleCommand>
{
    public UnassignUserRoleValidator()
    {
        RuleFor(e => e.UnassignUserRoleRequestDto.UserEmail)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

        RuleFor(e => e.UnassignUserRoleRequestDto.RoleName)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

    }
}
