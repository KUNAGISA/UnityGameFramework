using System;
using System.Collections.Generic;

namespace GameFramework
{
    public sealed class BindableProperty<T> : IBindableProperty<T>, IReadonlyBindableProperty<T>, ICanceller<Action<T>>
    {
        /// <summary>
        /// 默认使用<see cref="EqualityComparer{T}.Default"/>，可自定义比较
        /// </summary>
        public static IEqualityComparer<T> Comparer { get; set; } = EqualityComparer<T>.Default;

        private T _value = default;
        public T Value
        {
            get => _value;
            set
            {
                if (!Comparer.Equals(value, _value))
                {
                    _value = value;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        private event Action<T> OnValueChanged;

        public BindableProperty(T value = default)
        {
            _value = value;
        }

        public void SetValueSilently(T value)
        {
            _value = value;
        }

        public ICancelToken Register(Action<T> onValueChanged)
        {
            OnValueChanged += onValueChanged;
            return CancelToken.Get(this, onValueChanged);
        }

        public ICancelToken RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(_value);
            return Register(onValueChanged);
        }

        public void Cancel(Action<T> onValueChanged)
        {
            OnValueChanged -= onValueChanged;
        }

        public static implicit operator T (BindableProperty<T> property)
        {
            return property._value;
        }
    }
}