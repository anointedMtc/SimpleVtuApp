namespace SharedKernel.Application.Interfaces;

public interface ICacheServiceRedis
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
            where T : class;

    Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default)
        where T : class;

    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class;

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);

    Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
       where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan? cacheDuration, CancellationToken cancellationToken = default)
       where T : class;

    Task SetByteAsync<T>(string key, T value, TimeSpan? cacheDuration, CancellationToken cancellationToken = default)
       where T : class;

    Task SetExternalApiKeyAsync<T>(string key, T value, TimeSpan? cacheDuration, CancellationToken cancellationToken = default)
       where T : class;

    Task AdminCacheAsync<T>(string key, T value, TimeSpan? slidingExpiration, TimeSpan? absoluteExpiration, CancellationToken cancellationToken = default)
        where T : class;

    Task AdminCacheAsync<T>(string key, T value, TimeSpan? cacheDuration, CancellationToken cancellationToken = default)
        where T : class;

    Task<T?> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory,
       TimeSpan? expiration = null, CancellationToken cancellationToken = default)
      where T : class;

    Task RefreshCache(string cacheKey, CancellationToken cancellationToken = default);

}
