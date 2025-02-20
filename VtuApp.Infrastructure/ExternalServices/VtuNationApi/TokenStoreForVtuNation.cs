using ApplicationSharedKernel.Interfaces;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices;

namespace VtuApp.Infrastructure.ExternalServices.VtuNationApi;

public class TokenStoreForVtuNation : ITokenStoreForVtuNation
{
    private readonly ICacheServiceRedis _cacheServiceRedis;
    private readonly ILogger<TokenStoreForVtuNation> _logger;
    private readonly IGetTokenFromVtuNation _getTokenFromVtuNation;


    public static readonly TimeSpan DefaultTokenCacheDuration = TimeSpan.FromSeconds(120);
    private static readonly string ExternalApiCacheKey = "VtuNationApiCacheKey";

    public TokenStoreForVtuNation(ICacheServiceRedis cacheServiceRedis, 
        ILogger<TokenStoreForVtuNation> logger, 
        IGetTokenFromVtuNation getTokenFromVtuNation)
    {
        _cacheServiceRedis = cacheServiceRedis;
        _logger = logger;
        _getTokenFromVtuNation = getTokenFromVtuNation;
    }

    public async Task<string> GetVtuNationApiToken()
    {
        _logger.LogInformation("Fetching data for VtuNationApiToken from cache with key: {CacheKey}.", ExternalApiCacheKey);
        var cachedResult = await _cacheServiceRedis.GetAsync<LoginResponseVtuNation>(ExternalApiCacheKey);

        if (cachedResult is not null)
        {
            _logger.LogInformation("Cache hit for key: {CacheKey}.", ExternalApiCacheKey);

            await _cacheServiceRedis.RefreshCache(ExternalApiCacheKey);
            return cachedResult.Token!;
        }


        _logger.LogInformation("Cache miss for key: {CacheKey}. Requesting data from External Api provider {Name}", 
            ExternalApiCacheKey,
            "VtuNationApi");

        var externalResponse = await _getTokenFromVtuNation.GetVtuNationApiTokenAsync(new LoginRequestVtuNation() { Phone = "08109713734", Password = "anointedMtc" });

        if (externalResponse.IsSuccessful && externalResponse.Content.IsSuccessful == true)
        {
            var externalApiKeyHolder = new LoginResponseVtuNation
            {
                Token = externalResponse.Content?.Token
            };

            // we are caching it only when it is successful...
            await _cacheServiceRedis.SetExternalApiKeyAsync(
                    ExternalApiCacheKey,
                    externalApiKeyHolder,
                    DefaultTokenCacheDuration
            );

            _logger.LogInformation("Setting data to cache for key: {CacheKey}", ExternalApiCacheKey);

            return externalApiKeyHolder.Token!;
        }
        else
        {
            // if it isn't successfull, we would log it and return "null" as string to the Delegate Handler... but we would not cache it... 
            _logger.LogError("Error getting JWT from External Api Service Provider {Name} at {time}", 
                "VtuNationApi",
                DateTimeOffset.UtcNow);

            // we don't even want to do anything specifically considering the fact that we have used polly for resilience and retries... that should be enough

            return "null";
        }
    }
}
