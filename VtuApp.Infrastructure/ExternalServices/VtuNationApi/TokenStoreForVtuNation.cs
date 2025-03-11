using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices;

namespace VtuApp.Infrastructure.ExternalServices.VtuNationApi;

public class TokenStoreForVtuNation : ITokenStoreForVtuNation
{
    private readonly ICacheServiceRedis _cacheServiceRedis;
    private readonly ILogger<TokenStoreForVtuNation> _logger;
    private readonly IGetTokenFromVtuNation _getTokenFromVtuNation;


    public static readonly TimeSpan DefaultTokenCacheDuration = TimeSpan.FromMinutes(120);
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
            _logger.LogError("Error getting JWT from External Api Service Provider {Name} at {time}", 
                "VtuNationApi",
                DateTimeOffset.UtcNow);

            return "null";
        }
    }
}
