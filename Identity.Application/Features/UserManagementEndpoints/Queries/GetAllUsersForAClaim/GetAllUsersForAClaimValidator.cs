using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersForAClaim;

public class GetAllUsersForAClaimValidator : AbstractValidator<GetAllUsersForAClaimQuery>
{
    public GetAllUsersForAClaimValidator()
    {

    }
}
