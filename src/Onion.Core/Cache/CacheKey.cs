using Onion.Core.Extensions;
using System.Security.Cryptography;

namespace Onion.Core.Cache;

public class CacheKey
{
    public string Key { get; set; }

    public CacheKey(string repositoryName, string methodName, params object[] parameters)
    {
        StringBuilder cacheKeyBuilder = new StringBuilder();
        cacheKeyBuilder.Append($"{repositoryName}");
        cacheKeyBuilder.Append($"__{methodName}");
        cacheKeyBuilder.Append($"__{string.Join("_", (parameters is null || parameters.Length == 0) ? new string[] { "?" } : parameters)}");
        Key = cacheKeyBuilder.ToString().ToMd5Thumbprint();
    }
}