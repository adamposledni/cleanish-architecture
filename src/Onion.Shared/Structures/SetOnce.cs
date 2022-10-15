namespace Onion.Shared.Structures;

public class SetOnce<T>
{
    private T _value;
    private bool _hasValue;

    public T Value {
        get {
            if (!_hasValue) return default;
            return _value;
        }
        set {
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
