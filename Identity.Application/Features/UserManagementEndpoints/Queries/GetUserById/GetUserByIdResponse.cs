using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserById;

public class GetUserByIdResponse : ApiBaseResponse
{
    public ApplicationUserResponseDto UserResponseDto { get; set; }
}
