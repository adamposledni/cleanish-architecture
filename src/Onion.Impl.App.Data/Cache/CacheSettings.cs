namespace Onion.Impl.App.Data.Cache;
internal class CacheSettings
{
    public const string CONFIG_KEY = "MemoryCache";

    public int Lifetime { get; set; }
    public int Size { get; set; }
}
