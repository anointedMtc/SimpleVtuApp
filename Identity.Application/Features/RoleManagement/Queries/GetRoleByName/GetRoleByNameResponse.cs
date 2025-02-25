using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleByName;

public class GetRoleByNameResponse : ApiBaseResponse
{
    public ApplicationRoleResponseDto ApplicationRoleResponseDto { get; set; }
}
