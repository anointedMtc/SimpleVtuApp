using FluentValidation;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleById;

public class GetRoleByIdValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdValidator()
    {
        RuleFor(r => r.RoleId)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
