using Identity.Application.HelperClasses;
using MediatR;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleById;

public class GetRoleByIdQuery : ICachedQuery<GetRoleByIdResponse>
{
    public Guid RoleId { get; set; }

    public string CacheKey => CacheHelpers.GenerateGetRoleByIdQueryCacheKey(RoleId);

    public TimeSpan? Expiration => null;
}
