using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Onion.App.Data.Cache;
using Onion.Shared.Helpers;

namespace Onion.Impl.App.Data.Cache;

internal class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public CacheService(IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
    {
        _memoryCache = memoryCache;

        _cacheOptions = new MemoryCacheEntryOptions()
            .SetSize(1)
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheSettings.Value.Lifetime));
    }

    public async Task<TResult> UseCacheAsync<TResult>(CacheKey cacheKey, Func<Task<TResult>> valueProvider)
    {
        Guard.NotNull(cacheKey, nameof(cacheKey));
        Guard.NotNull(valueProvider, nameof(valueProvider));

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