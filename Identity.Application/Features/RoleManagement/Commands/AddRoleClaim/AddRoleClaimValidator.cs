using FluentValidation;

namespace Identity.Application.Features.RoleManagement.Commands.AddRoleClaim;

public class AddRoleClaimValidator : AbstractValidator<AddRoleClaimCommand>
{
    public AddRoleClaimValidator()
    {
        RuleFor(r => r.AppRoleId)
          .NotEmpty().WithMessage("{PropertyName} should have value");

        RuleFor(r => r.AddRoleClaimRequestDto.RoleClaims)
          .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
