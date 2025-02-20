using ApplicationSharedKernel.DTO;

namespace Identity.Application.Features.UsersEndpoints.RegisterUser;

public class RegisterUserResponse : ApiBaseResponse
{
    public string UserId { get; set; }
}
