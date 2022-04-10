using Microsoft.Extensions.Caching.Memory;
using Onion.Core.Cache;

namespace Onion.Infrastructure.Core.Cache;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        // TODO: cache settings as IOptions
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
            .SetSize(1024);

    }

    public async Task<T> UseCacheAsync<T>(CacheKey cacheKey, Func<Task<T>> valueProvider)
    {
        if (!_memoryCache.TryGetValue(cacheKey.Key, out T value))
        {
            value = await valueProvider();
            _memoryCache.Set(cacheKey.Key, value, _cacheOptions);
        }
        return value;
    }

    public void Remove(CacheKey cacheKey)
    {
        _memoryCache.Remove(cacheKey.Key);
    }
}