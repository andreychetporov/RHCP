using System;
using System.Collections.Generic;

public class ReactiveVariableClamped : ReactiveVariable<int>
{
    private int _minValue;
    private int _maxValue;

    public override int Value
    {
        get => base.Value;
        set => base.Value = Math.Clamp(value, _minValue, _maxValue);
    }

    public ReactiveVariableClamped(int minValue, int maxValue) : this(minValue, minValue, maxValue) { }
    public ReactiveVariableClamped(int value, int minValue, int maxValue) : base(Math.Clamp(value, minValue, maxValue), EqualityComparer<int>.Default)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }

    public bool CanAdd(int amount) => Value + amount >= _minValue && Value + amount <= _maxValue;
    public bool CanSubtract(int amount) => Value - amount >= _minValue && Value - amount <= _maxValue;
    public bool CanSet(int value) => value >= _minValue && value <= _maxValue;

    public override void Dispose()
    {
        base.Dispose();

        _maxValue = 0;
        _minValue = 0;
    }
}
