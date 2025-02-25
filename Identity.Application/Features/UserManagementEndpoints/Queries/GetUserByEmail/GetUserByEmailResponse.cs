using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserByEmail;

public class GetUserByEmailResponse : ApiBaseResponse
{
    public ApplicationUserResponseDto UserResponseDto { get; set; }

}
