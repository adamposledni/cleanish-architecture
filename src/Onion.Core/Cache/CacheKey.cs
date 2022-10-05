using Onion.Core.Extensions;

namespace Onion.Core.Cache;

public class CacheKey
{
    public string Key { get; set; }

    public CacheKey(string repositoryName, string methodName, params string[] parameters)
    {
        StringBuilder cacheKeyBuilder = new StringBuilder();
        cacheKeyBuilder.Append($"{repositoryName}");
        cacheKeyBuilder.Append($"__{methodName}");
        if (parameters != null && parameters.Length > 0)
        {
            cacheKeyBuilder.Append($"__{string.Join("_", parameters.Select(p => p == null ? "?" : p.ToString()))}");
        }
        
        Key = cacheKeyBuilder.ToString().ToMd5Thumbprint();
    }
}