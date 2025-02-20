using Identity.Shared.DTO;
using MediatR;

namespace Identity.Application.Features.UsersEndpoints.ForgotPassword;

public class ForgotPasswordCommand : IRequest<ForgotPasswordResponse>
{
    public ForgotPasswordRequestDto ForgotPasswordDto { get; set; }
}
