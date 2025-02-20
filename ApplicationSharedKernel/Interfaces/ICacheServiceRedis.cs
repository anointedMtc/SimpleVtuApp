namespace ApplicationSharedKernel.Interfaces;

public interface ICacheServiceRedis
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
            where T : class;

    // another implementation of GetAsync - BETTER - which encapsulates checking for the data, if not found, then it executes a fuction to get it and sets it inside the cache with it's key
    // this one will always return a result so it shouldn't be nullable
    Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default)
        where T : class;


    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class;



    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);



    // THE ONES I TRIED TO FORM BY MYSELF   
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

    // THE ONE THAT DOES THE GETORCREATEASYNC MAGIC IN THE PIPELINE BEHAVIOUR
    //Task<T?> GetAsync<T>(string key, Func<CancellationToken, Task<T>> factory,
    //    TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    //   where T : class;

    Task<T?> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory,
       TimeSpan? expiration = null, CancellationToken cancellationToken = default)
      where T : class;


    Task RefreshCache(string cacheKey, CancellationToken cancellationToken = default);

}
