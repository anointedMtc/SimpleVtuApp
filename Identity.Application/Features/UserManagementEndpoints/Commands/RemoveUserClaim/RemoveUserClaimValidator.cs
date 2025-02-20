using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.RemoveUserClaim;

public class RemoveUserClaimValidator : AbstractValidator<RemoveUserClaimCommand>
{
    public RemoveUserClaimValidator()
    {
        RuleFor(r => r.UserId)
          .NotEmpty().WithMessage("{PropertyName} should have value");

        RuleFor(r => r.RemoveUserClaimRequestDto.UserClaims)
         .NotEmpty().WithMessage("{PropertyName} should have value");
    }
}
