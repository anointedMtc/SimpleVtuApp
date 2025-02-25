using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;

public class GetUserClaimsResponse : ApiBaseResponse
{
    public GetUserClaimsResponseDto GetUserClaimsResponseDto { get; set; }
}
