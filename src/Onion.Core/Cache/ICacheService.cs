namespace Onion.Core.Cache;

public interface ICacheService
{
    Task<T> UseCacheAsync<T>(CacheKey cacheKey, Func<Task<T>> valueProvider);
}
