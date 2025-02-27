using MediatR;

namespace SharedKernel.Application.Interfaces;

public interface ICacheableAdmin<TResponse> : IRequest<TResponse>, ICacheableAdmin;

public interface ICacheableAdmin
{
    bool BypassCache { get; }
    string CacheKey { get; }
    TimeSpan? SlidingExpirationInMinutes { get; }
    TimeSpan? AbsoluteExpirationInMinutes { get; }

    // if you really want to make use of the Dto, then comment out the above individual fields/props and make use of the Dto below which encapsulates all of them
    //public AdminCacheControlDto AdminCacheControlDto { get; set; }

}

