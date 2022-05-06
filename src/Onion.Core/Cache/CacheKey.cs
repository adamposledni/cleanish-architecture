using Onion.Core.Extensions;
using System.Security.Cryptography;

namespace Onion.Core.Cache;

public class CacheKey
{
    public string Key { get; set; }

    public CacheKey(string repositoryName, string methodName, params object[] parameters)
    {
        StringBuilder cacheBuilder = new StringBuilder();
        cacheBuilder.Append($"{repositoryName}");
        cacheBuilder.Append($"__{methodName}");
        cacheBuilder.Append($"__{string.Join("_", (parameters is null || parameters.Length == 0) ? new string[] { "?" } : parameters)}");
        Key = cacheBuilder.ToString().ToMd5Thumbprint();
    }
}