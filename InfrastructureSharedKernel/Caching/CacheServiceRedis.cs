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


    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value, serializerOptions);

        await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);

        CacheKeys.TryAdd(key, false);  // the value true or false is irrelevant, we only want the key... this is necessary only if you want to remove by prefix
    }

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


    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {

        await _distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out bool _);       // to make sure we remove the same value from our cache keys dictionary... we pass in the key that we want to remove, we also have to specify and out parameter that is going to represent our boolean value but we are just going to use a discard parameter here because we don't care about the value that we removed from the cache
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {

        IEnumerable<Task> task = CacheKeys
            .Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(task);
    }


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


    private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = null,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
