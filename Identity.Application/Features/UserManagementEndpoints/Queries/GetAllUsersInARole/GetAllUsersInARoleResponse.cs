using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersInARole;

public class GetAllUsersInARoleResponse : ApiBaseResponse
{
    public List<ApplicationUserShortResponseDto> ApplicationUserShortResponseDto { get; set; }

}
