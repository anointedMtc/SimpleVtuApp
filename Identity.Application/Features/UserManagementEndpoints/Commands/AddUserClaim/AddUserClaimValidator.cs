using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.AddUserClaim;

public class AddUserClaimValidator : AbstractValidator<AddUserClaimCommand>
{
    public AddUserClaimValidator()
    {
        RuleFor(r => r.UserId)
          .NotEmpty().WithMessage("{PropertyName} should have value");

        RuleFor(r => r.AddUserClaimRequestDto.UserClaims)
          .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
