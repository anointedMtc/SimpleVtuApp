using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.UsersEndpoints.VerifyTwoFacAuth;

public class VerifyTwoFacAuthResponse : ApiBaseResponse
{
    public VerifyTwoFacAuthResponse()
    {
        VerifyTwoFacAuthResponseDto = new();
    }
    public VerifyTwoFacAuthResponseDto VerifyTwoFacAuthResponseDto { get; set; }
}
