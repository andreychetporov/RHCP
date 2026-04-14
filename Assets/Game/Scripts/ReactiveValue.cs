using System;
using System.Collections.Generic;

public class ReactiveVariable<T> : IDisposable
{
    public event Action<T, T> OnValueChanged;

    private T _value;
    private IEqualityComparer<T> _comparer;

    public ReactiveVariable() : this(default(T), EqualityComparer<T>.Default)
    {
    }

    public ReactiveVariable(T value) : this(value, EqualityComparer<T>.Default)
    {
    }

    public ReactiveVariable(T value, IEqualityComparer<T> comparer)
    {
        _value = value;
        _comparer = comparer;
    }

    public virtual T Value
    {
        get => _value;
        set
        {
            T oldValue = _value;

            _value = value;

            if (!_comparer.Equals(value, oldValue))
            {
                OnValueChanged?.Invoke(oldValue, _value);
            }
        }
    }

    public virtual void Dispose()
    {
        _comparer = default;
        _value = default;

        OnValueChanged = null;
    }
}