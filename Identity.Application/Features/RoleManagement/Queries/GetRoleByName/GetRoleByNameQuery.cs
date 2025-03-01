using Identity.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleByName;

public class GetRoleByNameQuery : ICachedQuery<GetRoleByNameResponse>
{
    public string Name { get; set; }

    public string CacheKey => CacheHelpers.GenerateGetRoleByNameQueryCacheKey(Name);

    public TimeSpan? Expiration => null;
}
