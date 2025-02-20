using MediatR;

namespace ApplicationSharedKernel.Interfaces;

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



/*

This interface includes properties that come handy while writing caching logics. 
Remember that this interface will be inherited by the MediatR Requests whenever 
required.

        BypassCache - determines if you want to skip caching and go directly to the database/datastore.

        CacheKey - specifies a unique cache key for each similar request

        SlidingExpirationInMinutes - if the cache record is not accessed for this period of time, it will be refreshed.

        AbsoluteExpirationInMinutes - time in minutes


*/