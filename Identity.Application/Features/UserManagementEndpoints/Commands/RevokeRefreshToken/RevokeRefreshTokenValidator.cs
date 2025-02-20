using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenValidator()
    {
        RuleFor(e => e.RevokeRefreshTokenRequestDto.RefreshToken)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

    }
}
