using ApplicationSharedKernel.Interfaces;
using ExternalServices.Application.TypiCodeService.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExternalServices.Application.TypiCodeService.Services;

public class AuthTokenStoreExternalApi : IAuthTokenStoreExternalApi
{
    private readonly ICacheServiceRedis _cacheServiceRedis;
    private readonly ILogger<AuthTokenStoreExternalApi> _logger;
    private readonly IGetJWTFromExternalApiTwo _getJWTFromExternalApiTwo;


    public static readonly TimeSpan DefaultTokenCacheDuration = TimeSpan.FromSeconds(120);
    private static readonly string ExternalApiCacheKey = "RWESpecPattern_ExternalApiCacheKey";      // this is because I used a prefix for all my cached keys... be conscious of this... it gave me a hard time here...


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

            // we want to reset its sliding expiration - but remember that its absolute expiration over-powers it so that once the absolute expiration time is up, it deletes the cache even if we just reset the sliding expiration just now
            await _cacheServiceRedis.RefreshCache(ExternalApiCacheKey);
            return cachedResult.AccessToken!;
        }


        _logger.LogInformation("Cache miss for key: {CacheKey}. Requesting data from External Api provider", ExternalApiCacheKey);

        var externalResponse = await _getJWTFromExternalApiTwo.GetUser(4);

        // The EnsureSuccessStatusCode method throws an exception if the HTTP response was unsuccessful. It can also call Dispose to free managed and unmanaged resources
        // do not use it because it will throw an HttpRequestException which you would have to catch or handle in the global exception handler.. instead use the .IsSuccessStatusCode property of the returned object to check for success and handle errors...
        //await externalResponse.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);


        // Since most Api responds with isSuccessful as boolean, you can check if it is true or false
        //if(externalResponse.IsSuccessful && externalResponse.Content.Email == "thisemail.com")
        if (externalResponse.IsSuccessful)
        {
            var externalApiKeyHolder = new ExternalApiKeyHolder();
            externalApiKeyHolder.AccessToken = externalResponse.Content?.Email;

            // we are caching it only when it is successful...
            await _cacheServiceRedis.SetExternalApiKeyAsync(
                    "ExternalApiCacheKey",
                    //externalResponse.Content.Email,
                    externalApiKeyHolder,
                    DefaultTokenCacheDuration
            );

            _logger.LogInformation("Setting data to cache for key: {CacheKey}", ExternalApiCacheKey);

            //return externalResponse.Content.Email;
            return externalApiKeyHolder.AccessToken!;
        }
        else
        {
            // if it isn't successfull, we would log it and return "null" as string to the Delegate Handler... but we would not cache it... 
            _logger.LogError("Error getting JWT from External Api Service Provider at {time}", DateTimeOffset.UtcNow);

            // we don't even want to do anything specifically considering the fact that we have used polly for resilience and retries... that should be enough

            //return "null"; or return "abc" as string
            //return null;
            return "null";
        }

    }
}
