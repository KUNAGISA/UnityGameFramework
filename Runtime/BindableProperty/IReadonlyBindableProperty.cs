using System;

namespace GameFramework
{
    public interface IReadonlyBindableProperty<T>
    {
        T Value { get; }

        SignalToken Register(Action<T> onValueChanged);

        SignalToken RegisterWithInitValue(Action<T> onValueChanged);
    }
}