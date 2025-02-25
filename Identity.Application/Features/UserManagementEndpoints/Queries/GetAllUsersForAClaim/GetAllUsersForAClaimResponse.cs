using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersForAClaim;

public class GetAllUsersForAClaimResponse : ApiBaseResponse
{
    public List<ApplicationUserShortResponseDto> ApplicationUserShortResponseDto { get; set; }
}
