using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.UsersEndpoints.AuthenticateUser;

public class AuthenticateUserResponse : ApiBaseResponse
{
    public LoginResponseDto LoginResponse { get; set; }
}
