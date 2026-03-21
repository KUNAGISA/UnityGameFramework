using System;
using System.Collections.Generic;

namespace GameFramework
{
    public sealed class BindableProperty<T> : IBindableProperty<T>, IReadonlyBindableProperty<T>
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
                    _onValueChanged?.Emit(_value);
                }
            }
        }

        private Signal<T> _onValueChanged;

        public BindableProperty(T value = default)
        {
            _value = value;
        }

        public void SetValueSilently(T value)
        {
            _value = value;
        }

        public SignalToken Register(Action<T> onValueChanged)
        {
            _onValueChanged ??= new Signal<T>();
            return _onValueChanged.Register(onValueChanged);
        }

        public SignalToken RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(_value);
            return Register(onValueChanged);
        }
        
        public static implicit operator T (BindableProperty<T> property)
        {
            return property._value;
        }
    }
}