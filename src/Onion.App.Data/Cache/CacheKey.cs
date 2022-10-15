using Onion.Shared.Extensions;

namespace Onion.App.Data.Cache;

public class CacheKey
{
    public string Key { get; set; }

    public CacheKey(string entityName, string methodName, params string[] parameters)
    {
        var cacheKeyBuilder = new StringBuilder();
        cacheKeyBuilder.Append($"{entityName}");
        cacheKeyBuilder.Append($"__{methodName}");
        if (parameters != null && parameters.Length > 0)
        {
            cacheKeyBuilder.Append($"__{string.Join("_", parameters.Select(p => p == null ? "?" : p.ToString()))}");
        }

        Key = cacheKeyBuilder.ToString().ToMd5Thumbprint();
    }
}