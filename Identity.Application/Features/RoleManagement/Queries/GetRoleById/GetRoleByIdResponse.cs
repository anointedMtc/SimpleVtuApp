using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleById;

public class GetRoleByIdResponse : ApiBaseResponse
{
    public ApplicationRoleResponseDto ApplicationRoleResponseDto { get; set; }
}
