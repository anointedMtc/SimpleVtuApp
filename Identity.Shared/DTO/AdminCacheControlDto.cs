namespace Identity.Shared.DTO;

public class AdminCacheControlDto
{
    public string CacheKey { get; set; }

    public bool BypassCache { get; set; }

    public int SlidingExpirationInMinutes { get; set; }

    public int AbsoluteExpirationInMinutes { get; set; }

}
