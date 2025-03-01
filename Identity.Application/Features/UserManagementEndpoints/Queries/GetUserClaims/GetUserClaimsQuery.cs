using Identity.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;

public class GetUserClaimsQuery : ICachedQuery<GetUserClaimsResponse>
{
    public Guid UserId { get; set; }

    public string CacheKey => CacheHelpers.GenerateGetUserClaimsQueryCacheKey(UserId);

    public TimeSpan? Expiration => null;
}
