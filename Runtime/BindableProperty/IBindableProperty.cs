using System;

namespace GameFramework
{
    public interface IBindableProperty<T>
    {
        T Value { get; set; }

        SignalToken Register(Action<T> onValueChanged);

        SignalToken RegisterWithInitValue(Action<T> onValueChanged);
        
        void SetValueSilently(T value);
    }
}