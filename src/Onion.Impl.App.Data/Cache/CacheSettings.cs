namespace Onion.Impl.App.Data.Cache;

public class CacheSettings
{
    public const string CONFIG_KEY = "MemoryCache";

    public int Lifetime { get; set; }
    public int Size { get; set; }
}
