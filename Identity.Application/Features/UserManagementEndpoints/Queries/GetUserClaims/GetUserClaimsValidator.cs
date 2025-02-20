using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;

public class GetUserClaimsValidator : AbstractValidator<GetUserClaimsQuery>
{
    public GetUserClaimsValidator()
    {
        RuleFor(r => r.UserId)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
