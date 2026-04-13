using System;

public class ReactiveValue<T>
{
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }
    }

    public event Action<T> OnChanged;

    public ReactiveValue(T initialValue)
    {
        _value = initialValue;
    }
}