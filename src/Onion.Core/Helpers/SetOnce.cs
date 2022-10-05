namespace Onion.Core.Helpers;

public class SetOnce<T>
{
    private T _value;
    private bool _hasValue;

    public T Value 
    {
        get 
        {
            if (!_hasValue) return default(T);
            return _value;
        }
        set 
        {
            if (_hasValue) throw new InvalidOperationException("Value already set");
            _value = value;
            _hasValue = true;
        }
    }

    public static implicit operator T(SetOnce<T> value) 
    { 
        return value.Value; 
    }
}
