using FluentValidation;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleByName;

public class GetRoleByNameValidator : AbstractValidator<GetRoleByNameQuery>
{
    public GetRoleByNameValidator()
    {
        RuleFor(r => r.Name)
           .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
