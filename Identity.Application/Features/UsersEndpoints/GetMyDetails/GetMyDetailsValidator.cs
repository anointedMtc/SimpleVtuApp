using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.GetMyDetails;

public class GetMyDetailsValidator : AbstractValidator<GetMyDetailsQuery>
{
    public GetMyDetailsValidator()
    {

    }
}
