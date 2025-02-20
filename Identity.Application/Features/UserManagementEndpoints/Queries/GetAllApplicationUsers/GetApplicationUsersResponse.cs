using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllApplicationUsers;

public class GetApplicationUsersResponse : ApiBaseResponse
{
    public List<ApplicationUserShortResponseDto> ApplicationUserShortResponseDto { get; set; }

}
