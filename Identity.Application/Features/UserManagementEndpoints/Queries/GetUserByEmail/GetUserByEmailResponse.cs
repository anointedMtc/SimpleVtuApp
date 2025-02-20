using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserByEmail;

public class GetUserByEmailResponse : ApiBaseResponse
{
    public ApplicationUserResponseDto UserResponseDto { get; set; }

}
