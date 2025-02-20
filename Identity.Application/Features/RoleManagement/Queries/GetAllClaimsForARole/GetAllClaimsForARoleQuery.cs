using MediatR;

namespace Identity.Application.Features.RoleManagement.Queries.GetAllClaimsForARole;

public class GetAllClaimsForARoleQuery : IRequest<GetAllClaimsForARoleResponse>
{
    public Guid RoleId { get; set; }
}
