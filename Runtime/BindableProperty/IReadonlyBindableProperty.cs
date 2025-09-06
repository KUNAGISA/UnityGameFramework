using System;

namespace GameFramework
{
    public interface IReadonlyBindableProperty<T>
    {
        T Value { get; }

        ICancelToken Register(Action<T> onValueChanged);

        ICancelToken RegisterWithInitValue(Action<T> onValueChanged);

        void Cancel(Action<T> onValueChanged);
    }
}