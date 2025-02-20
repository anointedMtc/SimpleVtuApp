using ApplicationSharedKernel.Interfaces;

namespace Identity.Application.Features.RoleManagement.Queries.GetRoleByName;

public class GetRoleByNameQuery : ICachedQuery<GetRoleByNameResponse>
{
    public string Name { get; set; }

    public string CacheKey => $"role-by-name-{Name}";

    public TimeSpan? Expiration => null;
}
