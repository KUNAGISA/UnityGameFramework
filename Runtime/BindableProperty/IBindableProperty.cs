using System;

namespace GameFramework
{
    public interface IBindableProperty<T>
    {
        T Value { get; set; }

        ICancelToken Register(Action<T> onValueChanged);

        ICancelToken RegisterWithInitValue(Action<T> onValueChanged);

        void Cancel(Action<T> onValueChanged);

        void SetValueSilently(T value);
    }
}