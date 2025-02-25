using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;

public class GetUserClaimsQuery : ICachedQuery<GetUserClaimsResponse>
{
    public Guid UserId { get; set; }

    public string CacheKey => $"User-by-Id-{UserId}";

    public TimeSpan? Expiration => null;
}
