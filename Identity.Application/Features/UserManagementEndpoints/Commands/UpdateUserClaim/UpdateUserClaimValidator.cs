using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UpdateUserClaim;

public class UpdateUserClaimValidator : AbstractValidator<UpdateUserClaimCommand>
{
    public UpdateUserClaimValidator()
    {
        RuleFor(r => r.UserId)
         .NotEmpty().WithMessage("{PropertyName} should have value");

        RuleFor(r => r.UpdateUserClaimRequestDto.UserClaims)
         .NotEmpty().WithMessage("{PropertyName} should have value");
    }
}
