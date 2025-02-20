
using ApplicationSharedKernel.Interfaces;

namespace Identity.Application.Features.UsersEndpoints.GetMyDetails;

public class GetMyDetailsQuery : ICachedQuery<GetMyDetailsResponse>
{
    public string UserId { get; set; }
    public string CacheKey => $"User-by-Id-{UserId}";
    public TimeSpan? Expiration => null;
}
