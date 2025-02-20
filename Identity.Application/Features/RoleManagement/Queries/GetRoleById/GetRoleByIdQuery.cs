using ApplicationSharedKernel.Interfaces;
using MediatR;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleById;

public class GetRoleByIdQuery : ICachedQuery<GetRoleByIdResponse>
{
    public Guid RoleId { get; set; }

    public string CacheKey => $"role-by-id-{RoleId}";

    public TimeSpan? Expiration => null;
}
