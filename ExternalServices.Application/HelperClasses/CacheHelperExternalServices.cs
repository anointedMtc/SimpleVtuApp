using SharedKernel.Domain.HelperClasses;

namespace ExternalServices.Application.HelperClasses;

public static class CacheHelperExternalServices
{
    public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
    private static readonly string _getAllPostsQueryKeyTemplate = "posts-{0}-{1}-{2}-{3}";
    private static readonly string _getPostByIdQueryKeyTemplate = "postById-{0}-{1}-{2}-{3}";



    public static string GenerateGetAllPostsQueryCacheKey()
    {
        return string.Format(_getAllPostsQueryKeyTemplate);
    }

    public static string GenerateGetPostByIdQueryCacheKey(int id)
    {
        return string.Format(_getPostByIdQueryKeyTemplate, id);
    }
}
