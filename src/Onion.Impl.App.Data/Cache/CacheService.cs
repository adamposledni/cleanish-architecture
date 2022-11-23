using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Entities;
using Onion.Shared.Helpers;
using System.Threading;

namespace Onion.Impl.App.Data.Cache;

internal sealed class CacheService<TEntity> : ICacheService<TEntity>, IDisposable where TEntity: BaseEntity
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;
    private CancellationTokenSource _resetCacheToken = new();
    //private CancellationTokenSource _resetCacheToken = new(TimeSpan.FromMilliseconds(50));
    private bool _disposed = false;

    public CacheService(IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
    {
        _memoryCache = memoryCache;
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetSize(1)
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheSettings.Value.Lifetime))
            .AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
    }

    public async Task<TResult> UseCacheAsync<TResult>(CacheKey cacheKey, Func<Task<TResult>> valueProvider)
    {
        Guard.NotNull(cacheKey, nameof(cacheKey));
        Guard.NotNullOrEmpty(cacheKey.Key, nameof(cacheKey.Key));
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
        Guard.NotNull(cacheKey.Key, nameof(cacheKey.Key));
        _memoryCache.Remove(cacheKey.Key);
    }

    public void Clear()
    {
        _resetCacheToken.Cancel();
        _resetCacheToken.Dispose();
        _resetCacheToken = new();
    }

    public void Dispose()
    {
        if (_disposed) return;
        if (_resetCacheToken != null)
        {
            _resetCacheToken.Dispose();
            _resetCacheToken = null;
        }
        _disposed = true;
    }
}