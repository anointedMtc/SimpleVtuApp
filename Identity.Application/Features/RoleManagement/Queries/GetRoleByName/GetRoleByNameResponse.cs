using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleByName;

public class GetRoleByNameResponse : ApiBaseResponse
{
    public ApplicationRoleResponseDto ApplicationRoleResponseDto { get; set; }
}
