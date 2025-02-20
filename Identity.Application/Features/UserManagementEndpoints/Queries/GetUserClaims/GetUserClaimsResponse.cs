using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;

public class GetUserClaimsResponse : ApiBaseResponse
{
    public GetUserClaimsResponseDto GetUserClaimsResponseDto { get; set; }
}
