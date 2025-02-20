using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.ResetPassword;

public class ResetPasswordCommand : IRequest<ResetPasswordResponse>
{
    public ResetPasswordRequestDto ResetPasswordDto { get; set; }
}
