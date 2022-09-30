using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Core.Cache;
using Onion.Core.Helpers;
using Onion.Core.Pagination;

namespace Onion.Infrastructure.DataAccess.Database.Repositories;

// TODO: use decorator pattern
public class CachedDatabaseRepository<T> : DatabaseRepository<T>, ICachedDatabaseRepository<T> where T: BaseEntity
{
    protected readonly ICacheService _cacheService;

    public CachedDatabaseRepository(SqlDbContext dbContext, ICacheService cacheService) : base(dbContext)
    {
        _cacheService = cacheService;
    }

    public async Task<T> CachedGetByIdAsync(Guid entityId)
    {
        return await _cacheService.UseCacheAsync(
            BuildCacheKey(nameof(CachedGetByIdAsync), entityId),
            () => GetByIdAsync(entityId));
    }

    public async Task<IEnumerable<T>> CachedListAsync()
    {
        return await _cacheService.UseCacheAsync(
            BuildCacheKey(nameof(CachedListAsync)),
            () => ListAsync());
    }

    public async Task<PaginableList<T>> CachedPaginateAsync(int pageSize, int page)
    {
        Guard.Min(pageSize, 1, nameof(pageSize));
        Guard.Min(page, 1, nameof(page));

        return await _cacheService.UseCacheAsync(
            BuildCacheKey(nameof(CachedPaginateAsync), pageSize, page),
            () => PaginateAsync(pageSize, page));
    }

    public async Task<int> CachedCountAsync()
    {
        return await _cacheService.UseCacheAsync(
            BuildCacheKey(nameof(CachedCountAsync)),
            () => CountAsync());
    }

    protected CacheKey BuildCacheKey(string methodName, params object[] parameters)
    {
        return new CacheKey(GetType().Name, methodName, parameters);
    }
}