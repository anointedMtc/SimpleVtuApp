using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.AuthenticateUser;

public class AuthenticateUserCommand : IRequest<AuthenticateUserResponse>
{
    public LoginRequestDto UserForAuthentication { get; set; }
}
