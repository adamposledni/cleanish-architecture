using Microsoft.Extensions.Caching.Memory;
using Onion.Core.Cache;
using Onion.Core.Helpers;

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
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
            .SetSize(1024);
    }

    public async Task<TResult> UseCacheAsync<TResult>(CacheStrategy cacheStrategy, CacheKey cacheKey, Func<Task<TResult>> valueProvider)
    {
        Guard.NotNull(cacheKey, nameof(cacheKey));

        if (cacheStrategy == CacheStrategy.Bypass) return await valueProvider();

        if (!_memoryCache.TryGetValue(cacheKey.Key, out TResult value))
        {
            value = await valueProvider();
            _memoryCache.Set(cacheKey.Key, value, _cacheOptions);
        }
        return value;
    }

    public void Remove(CacheKey cacheKey)
    {
        Guard.NotNull(cacheKey, nameof(cacheKey));
        _memoryCache.Remove(cacheKey.Key);
    }
}