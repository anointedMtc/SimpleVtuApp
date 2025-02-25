using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using SharedKernel.Application.Interfaces;

namespace SharedKernel.Infrastructure.Caching;

public class CacheServiceRedis : ICacheServiceRedis
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();  // necessary for removing by prefix... chose this dataType because it is threadSafe and chose bool because of low memory footprint.. 1 byte it can only be true or false

    private readonly IDistributedCache _distributedCache;

    public CacheServiceRedis(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    // NO.1
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cachedValue is null)
        {
            return null;
        }

        T? value = JsonSerializer.Deserialize<T>(cachedValue, serializerOptions);

        return value;
    }


    // ORIGINAL IMPLEMENTATION BY MILAN JOVANOVIC
    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value, serializerOptions);

        await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);

        CacheKeys.TryAdd(key, false);  // the value true or false is irrelevant, we only want the key... this is necessary only if you want to remove by prefix
    }

    // NO.2
    // COMBINING KNOWLEDGE FROM MILAN AND CODEMAZE      - and this one is actually better
    public async Task SetAsync<T>(string key, T value, TimeSpan? cacheDuration, CancellationToken cancellationToken = default) where T : class
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration ?? DefaultExpiration,
            SlidingExpiration = TimeSpan.FromMinutes(1)
        };

        //string cacheValue = JsonSerializer.Serialize(value);
        string cacheValue = JsonSerializer.Serialize(value, serializerOptions);

        await _distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);

        CacheKeys.TryAdd(key, false);  // the value true or false is irrelevant, we only want the key... this is necessary only if you want to remove by prefix
    }


    // NO.3
    // ADDITIONAL COMBINATION OF KNOWLEDGE FROM CODE-WITH-MUKESH    - ordinarily this should be the same the SetAsync method but i Had to rename it because it now made it two methods with the same name and parameters which the compiler complained... but the key difference is that we are setting byte here not string
    public async Task SetByteAsync<T>(string key, T value, TimeSpan? cacheDuration, CancellationToken cancellationToken = default) where T : class
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration ?? DefaultExpiration,
            SlidingExpiration = TimeSpan.FromMinutes(1)
        };

        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, serializerOptions));
        await _distributedCache.SetAsync(key, bytes, options, cancellationToken);

        CacheKeys.TryAdd(key, false);  // the value true or false is irrelevant, we only want the key... this is necessary only if you want to remove by prefix
    }

    // N0. 7
    // FOR EXTERNAL API CACHE   - maybe the name could have been SetWithCustomTimeAsync so that we can use it whenever we want to specifically say the cache time we want
    public async Task SetExternalApiKeyAsync<T>(string key, T value, TimeSpan? cacheDuration, CancellationToken cancellationToken = default) where T : class
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration + TimeSpan.FromHours(2), // adding additional two hours just to make sure that the absolute expiration would always be greater than the sliding expiration
            SlidingExpiration = cacheDuration
        };

        //string cacheValue = JsonSerializer.Serialize(value);
        string cacheValue = JsonSerializer.Serialize(value, serializerOptions);

        await _distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);

        CacheKeys.TryAdd(key, false);  // the value true or false is irrelevant, we only want the key... this is necessary only if you want to remove by prefix
    }


    // NO.4
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {

        await _distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out bool _);       // to make sure we remove the same value from our cache keys dictionary... we pass in the key that we want to remove, we also have to specify and out parameter that is going to represent our boolean value but we are just going to use a discard parameter here because we don't care about the value that we removed from the cache
    }

    // NO.5
    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {

        IEnumerable<Task> task = CacheKeys
            .Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(task);
    }


    // ORIGINAL IMPLEMENTATION BY MILAN
    // the implementation that is better   -  this is exactly the same behaviour we have for the GetOrCreateAsync we implemented earlier with IMemoryCache... it checks for the data if it isn't present, then it executes factory func to go ahead and get the data then set it in the memory of caches
    public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        cachedValue = await factory();

        await SetAsync(key, cachedValue, cancellationToken);

        return cachedValue;
    }



    // COMBINING KNOWLEDGE FROM MILAN AND CODEMAZE

    public async Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
    {
        T? cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        cachedValue = await factory();

        await SetAsync(key, cachedValue, expiration, cancellationToken);

        return cachedValue;
    }


    // NO.6
    // THIS IS THE ONE THAT DOES THE GETORCREATEASYNC METHOD USED IN THE PIPELINE
    // name this method GetOrSetAsync
    // from codeWithMukesh     --  --- --- This is a wrapper around both of the above extensions. Basically, in this single method, it handles both the Get and Set operations flawlessly. If the Cache Key is found in Redis, it returns the data. And if it’s not found, it executes the passed task (lambda function), and sets the returned value to the cache memory.
    public async Task<T?> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
    {
        // HERE WE CALL THE GET METHOD
        T? cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        // IF IT IS NULL, THEN PROCEED WITH THE ORIGINAL QUERY
        cachedValue = await factory(cancellationToken);

        // WHEN IT COMES BACK SUCCESSFULLY THEN CACHE IT BY CALLING ON THE SET METHOD   - SO IT IS ALL IN ONE
        if (cachedValue is not null)
        {
            await SetAsync(key, cachedValue, expiration, cancellationToken);
        }

        return cachedValue;
    }

    // refreshes a value in the cache based on its key, RESETTING ITS SLIDING EXPIRATION TIMEOUT IF ANY
    // we want to reset its sliding expiration - but remember that it absolute expiration over-powers it so that once the absolute expiration time is up, it deletes the cache even if we just reset the sliding expiration just now
    public async Task RefreshCache(string cacheKey, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RefreshAsync(cacheKey, cancellationToken);
    }

    public async Task AdminCacheAsync<T>(string key, T value, TimeSpan? slidingExpiration, TimeSpan? absoluteExpiration, CancellationToken cancellationToken = default) where T : class
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration ?? TimeSpan.FromMinutes(2),
            SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(1)
        };

        string cacheValue = JsonSerializer.Serialize(value, serializerOptions);

        await _distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);

        CacheKeys.TryAdd(key, false);  // the value true or false is irrelevant, we only want the key... this is necessary only if you want to remove by prefix

    }

    public async Task AdminCacheAsync<T>(string key, T value, TimeSpan? cacheDuration, CancellationToken cancellationToken = default) where T : class
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration + TimeSpan.FromMinutes(2), // adding additional two Minutes just to make sure that the absolute expiration would always be greater than the sliding expiration
            SlidingExpiration = cacheDuration
        };

        string cacheValue = JsonSerializer.Serialize(value, serializerOptions);

        await _distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);

        CacheKeys.TryAdd(key, false);  // the value true or false is irrelevant, we only want the key... this is necessary only if you want to remove by prefix

    }


    // this additional part is from codeWithMukesh
    private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = null,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
