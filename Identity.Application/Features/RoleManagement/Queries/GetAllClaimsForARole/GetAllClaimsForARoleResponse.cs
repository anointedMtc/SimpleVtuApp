using Identity.Shared.DTO;
using SharedKernel.Application.DTO;

namespace Identity.Application.Features.RoleManagement.Queries.GetAllClaimsForARole;

public class GetAllClaimsForARoleResponse : ApiBaseResponse
{
    //public GetAllClaimsForARoleResponse()
    //{
    //    //To Avoid runtime exception, we are initializing the class property
    //    GetAllClaimsForARoleResponseDto = new GetAllClaimsForARoleResponseDto();
    //}
    public GetAllClaimsForARoleResponseDto GetAllClaimsForARoleResponseDto { get; set; }
}
