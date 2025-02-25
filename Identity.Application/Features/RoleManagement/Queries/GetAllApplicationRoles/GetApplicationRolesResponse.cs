using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.RoleManagement.Queries.GetAllApplicationRoles;

public class GetApplicationRolesResponse : ApiBaseResponse
{
    public List<ApplicationRoleResponseDto> ApplicationRoleResponseDto { get; set; }
}
