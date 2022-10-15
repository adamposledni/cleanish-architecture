namespace Onion.App.Data.Cache;

public interface ICacheService
{
    Task<T> UseCacheAsync<T>(CacheKey cacheKey, Func<Task<T>> valueProvider);
}
