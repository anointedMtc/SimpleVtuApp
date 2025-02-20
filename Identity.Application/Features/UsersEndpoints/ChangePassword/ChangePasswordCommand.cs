using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.ChangePassword;

public class ChangePasswordCommand : IRequest<ChangePasswordResponse>
{
    public ChangePasswordRequestDto ChangePasswordRequestDto { get; set; }
}
