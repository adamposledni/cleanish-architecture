namespace Cleanish.App.Data.Cache;

public class Cached<T> where T : ICachable
{
    public T Value { get; }

    public Cached(T value)
    {
        Value = value;
    }
}
