using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.UsersEndpoints.GetMyDetails;

public class GetMyDetailsResponse : ApiBaseResponse
{
    public ApplicationUserResponseDto UserResponseDto { get; set; }

}
