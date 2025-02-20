using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.RoleManagement.Queries.GetAllApplicationRoles;

public class GetApplicationRolesResponse : ApiBaseResponse
{
    public List<ApplicationRoleResponseDto> ApplicationRoleResponseDto { get; set; }
}
