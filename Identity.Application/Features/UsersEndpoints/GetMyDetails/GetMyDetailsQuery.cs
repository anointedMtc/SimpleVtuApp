using Identity.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UsersEndpoints.GetMyDetails;

public class GetMyDetailsQuery : ICachedQuery<GetMyDetailsResponse>
{
    public string UserId { get; set; }
    public string CacheKey => CacheHelpers.GenerateGetMyDetailsQueryCacheKey(UserId);  
    public TimeSpan? Expiration => null;
}
