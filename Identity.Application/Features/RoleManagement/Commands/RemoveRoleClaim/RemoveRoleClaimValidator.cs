using FluentValidation;

namespace Identity.Application.Features.RoleManagement.Commands.RemoveRoleClaim;

public class RemoveRoleClaimValidator : AbstractValidator<RemoveRoleClaimCommand>
{
    public RemoveRoleClaimValidator()
    {
        RuleFor(r => r.AppRoleId)
          .NotEmpty().WithMessage("{PropertyName} should have value");

        RuleFor(r => r.RemoveRoleClaimRequestDto.RoleClaims)
          .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
