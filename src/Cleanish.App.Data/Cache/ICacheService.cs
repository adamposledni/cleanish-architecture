using Cleanish.App.Data.Database.Entities;

namespace Cleanish.App.Data.Cache;

public interface ICacheService<TEntity> where TEntity : BaseEntity
{
    Task<T> UseCacheAsync<T>(CacheKey cacheKey, Func<Task<T>> valueProvider);
    void Remove(CacheKey cacheKey);
    void Clear();
}
