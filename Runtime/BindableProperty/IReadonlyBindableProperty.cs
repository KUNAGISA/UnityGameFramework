using System;

namespace Aoiro
{
    public interface IReadonlyBindableProperty<T>
    {
        T Value { get; }

        SignalToken Register(Action<T> onValueChanged);

        SignalToken RegisterWithInitValue(Action<T> onValueChanged);
    }
}