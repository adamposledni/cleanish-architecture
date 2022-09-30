namespace Onion.Core.Cache;

public interface ICacheService
{
    Task<T> UseCacheAsync<T>(CacheStrategy cacheStrategy, CacheKey cacheKey, Func<Task<T>> valueProvider);
}
