using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserById;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(r => r.UserId)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
