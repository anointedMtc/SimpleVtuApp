using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(e => e.RefreshTokenRequest.RefreshToken)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

    }
}
