namespace Onion.App.Data.Cache;

public interface ICacheService<TEntity>
{
    Task<T> UseCacheAsync<T>(CacheKey cacheKey, Func<Task<T>> valueProvider);
    void Remove(CacheKey cacheKey);
    void Clear();
}
