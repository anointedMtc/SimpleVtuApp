using ApplicationSharedKernel.DTO;
using Identity.Shared.DTO;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersForAClaim;

public class GetAllUsersForAClaimResponse : ApiBaseResponse
{
    public List<ApplicationUserShortResponseDto> ApplicationUserShortResponseDto { get; set; }
}
