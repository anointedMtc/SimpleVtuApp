using ExternalServices.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace ExternalServices.Application.TypiCodeService.Features.GetAllPosts;

public class GetAllPostsQuery : ICachedQuery<GetAllPostsResponse>
{
    public string CacheKey => CacheHelperExternalServices.GenerateGetAllPostsQueryCacheKey();
    //public string CacheKey => $"all-posts---"; 

    public TimeSpan? Expiration => null;
}
