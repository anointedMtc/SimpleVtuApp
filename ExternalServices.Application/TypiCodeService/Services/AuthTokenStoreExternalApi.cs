using ExternalServices.Application.TypiCodeService.Interfaces;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace ExternalServices.Application.TypiCodeService.Services;

public class AuthTokenStoreExternalApi : IAuthTokenStoreExternalApi
{
    private readonly ICacheServiceRedis _cacheServiceRedis;
    private readonly ILogger<AuthTokenStoreExternalApi> _logger;
    private readonly IGetJWTFromExternalApiTwo _getJWTFromExternalApiTwo;


    public static readonly TimeSpan DefaultTokenCacheDuration = TimeSpan.FromSeconds(120);
    private static readonly string ExternalApiCacheKey = "RWESpecPattern_ExternalApiCacheKey";      


    public AuthTokenStoreExternalApi(ICacheServiceRedis cacheServiceRedis,
        ILogger<AuthTokenStoreExternalApi> logger,
        IGetJWTFromExternalApiTwo getJWTFromExternalApiTwo)
    {
        _cacheServiceRedis = cacheServiceRedis;
        _logger = logger;
        _getJWTFromExternalApiTwo = getJWTFromExternalApiTwo;
    }

    public async Task<string> GetExterlApiToken()
    {
        // THIS ONE IS A TWO STEP PROCESS
        _logger.LogInformation("Fetching data for ExternalApiToken from cache with key: {CacheKey}.", ExternalApiCacheKey);
        var cachedResult = await _cacheServiceRedis.GetAsync<ExternalApiKeyHolder>("ExternalApiCacheKey");

        if (cachedResult is not null)
        {
            _logger.LogInformation("Cache hit for key: {CacheKey}.", ExternalApiCacheKey);

            await _cacheServiceRedis.RefreshCache(ExternalApiCacheKey);
            return cachedResult.AccessToken!;
        }


        _logger.LogInformation("Cache miss for key: {CacheKey}. Requesting data from External Api provider", ExternalApiCacheKey);

        var externalResponse = await _getJWTFromExternalApiTwo.GetUser(4);


        if (externalResponse.IsSuccessful)
        {
            var externalApiKeyHolder = new ExternalApiKeyHolder();
            externalApiKeyHolder.AccessToken = externalResponse.Content?.Email;

            await _cacheServiceRedis.SetExternalApiKeyAsync(
                    "ExternalApiCacheKey",
                    externalApiKeyHolder,
                    DefaultTokenCacheDuration
            );

            _logger.LogInformation("Setting data to cache for key: {CacheKey}", ExternalApiCacheKey);

            return externalApiKeyHolder.AccessToken!;
        }
        else
        {
            _logger.LogError("Error getting JWT from External Api Service Provider at {time}", DateTimeOffset.UtcNow);

            return "null";
        }
    }
}
