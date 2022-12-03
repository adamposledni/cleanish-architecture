namespace Cleanish.App.Data.Cache;

public interface ICachable
{
    CacheStrategy CacheStrategy { get; }
}
