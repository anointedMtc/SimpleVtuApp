using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.UsersEndpoints.AuthenticateUser;

public class AuthenticateUserResponse : ApiBaseResponse
{
    public LoginResponseDto LoginResponse { get; set; }
}
