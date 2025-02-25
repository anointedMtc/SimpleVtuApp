using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersInARole;

public class GetAllUsersInARoleResponse : ApiBaseResponse
{
    public List<ApplicationUserShortResponseDto> ApplicationUserShortResponseDto { get; set; }

}
