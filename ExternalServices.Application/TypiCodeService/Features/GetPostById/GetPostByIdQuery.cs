using ExternalServices.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace ExternalServices.Application.TypiCodeService.Features.GetPostById;

public class GetPostByIdQuery : ICachedQuery<GetPostByIdResponse>
{
    public int Id { get; set; }

    public string CacheKey => CacheHelperExternalServices.GenerateGetPostByIdQueryCacheKey(Id);
    //public string CacheKey => $"posts-by{Id}";

    public TimeSpan? Expiration => null;
}
