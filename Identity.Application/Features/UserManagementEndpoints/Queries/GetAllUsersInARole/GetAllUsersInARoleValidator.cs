using FluentValidation;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersInARole;

public class GetAllUsersInARoleValidator : AbstractValidator<GetAllUsersInARoleQuery>
{
    public GetAllUsersInARoleValidator()
    {

    }
}
