using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.UsersEndpoints.GetMyDetails;

public class GetMyDetailsResponse : ApiBaseResponse
{
    public ApplicationUserResponseDto UserResponseDto { get; set; }

}
