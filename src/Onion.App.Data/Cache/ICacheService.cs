using Onion.App.Data.Database.Entities;

namespace Onion.App.Data.Cache;

public interface ICacheService<TEntity> where TEntity : BaseEntity
{
    Task<T> UseCacheAsync<T>(CacheKey cacheKey, Func<Task<T>> valueProvider);
    void Remove(CacheKey cacheKey);
    void Clear();
}
