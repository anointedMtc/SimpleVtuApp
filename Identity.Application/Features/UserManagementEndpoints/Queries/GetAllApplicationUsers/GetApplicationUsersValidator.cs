using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllApplicationUsers;

public class GetApplicationUsersValidator : AbstractValidator<GetApplicationUsersQuery>
{
    public GetApplicationUsersValidator()
    {

    }
}
