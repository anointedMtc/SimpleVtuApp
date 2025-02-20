using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleById;

public class GetRoleByIdResponse : ApiBaseResponse
{
    public ApplicationRoleResponseDto ApplicationRoleResponseDto { get; set; }
}
