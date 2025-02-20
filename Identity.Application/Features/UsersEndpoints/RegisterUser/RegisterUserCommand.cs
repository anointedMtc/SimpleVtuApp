using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.RegisterUser;

public class RegisterUserCommand : IRequest<RegisterUserResponse>
{
    public ApplicationUserRegisterationRequestDto UserForRegisteration { get; set; }
}
