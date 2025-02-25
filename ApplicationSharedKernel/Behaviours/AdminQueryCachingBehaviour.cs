using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;
using SharedKernel.Application.Interfaces;

namespace SharedKernel.Application.Behaviours;

internal sealed class AdminQueryCachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICacheableAdmin
        where TResponse : class
{
    private readonly ICacheServiceRedis _cacheServiceRedis;
    private readonly ILogger _logger;

    public AdminQueryCachingBehaviour(ICacheServiceRedis cacheServiceRedis, ILogger<AdminQueryCachingBehaviour<TRequest, TResponse>> logger)
    {
        _cacheServiceRedis = cacheServiceRedis;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        if (request.BypassCache) return await next();

        _logger.LogInformation("fetching data for key: {CacheKey} from cache.", request.CacheKey);

        TResponse? cachedResult = await _cacheServiceRedis.GetAsync<TResponse>(
            request.CacheKey,
            cancellationToken);

        if (cachedResult is not null)
        {
            _logger.LogInformation("Cache hit for key: {CacheKey}.", request.CacheKey);

            // we want to reset its sliding expiration - but remember that it absolute expiration over-powers it so that once the absolute expiration time is up, it deletes the cache even if we just reset the sliding expiration just now
            await _cacheServiceRedis.RefreshCache(request.CacheKey, cancellationToken);

            return cachedResult;
        }


        TResponse result = await next();

        _logger.LogInformation("Cache miss for key: {CacheKey} fetching data from database.", request.CacheKey);


        await _cacheServiceRedis.AdminCacheAsync(
            request.CacheKey,
            result,
            request.SlidingExpirationInMinutes,
            request.AbsoluteExpirationInMinutes,
            cancellationToken);

        _logger.LogInformation("setting data for key: {CacheKey} to cache.", request.CacheKey);

        return result;
    }
}