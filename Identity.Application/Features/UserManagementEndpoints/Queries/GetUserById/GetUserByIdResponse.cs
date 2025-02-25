using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserById;

public class GetUserByIdResponse : ApiBaseResponse
{
    public ApplicationUserResponseDto UserResponseDto { get; set; }
}
