using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IReadonlyBindableProperty<T>
    {
        T Value { get; }

        IUnRegister Register(Action<T> onValueChanged);

        IUnRegister RegisterWithInitValue(Action<T> onValueChanged);

        void UnRegister(Action<T> onValueChanged);
    }

    public interface IBindableProperty<T>
    {
        T Value { get; set; }

        IUnRegister Register(Action<T> onValueChanged);

        IUnRegister RegisterWithInitValue(Action<T> onValueChanged);

        void UnRegister(Action<T> onValueChanged);

        void SetValueSilently(T value);
    }

    public sealed class BindableProperty<T> : IBindableProperty<T>, IReadonlyBindableProperty<T>, IUnRegisterable<Action<T>>
    {
        /// <summary>
        /// 默认使用<see cref="EqualityComparer{T}.Default"/>，可自定义比较
        /// </summary>
        public static IEqualityComparer<T> Comparer { get; set; } = EqualityComparer<T>.Default;

        private T m_value = default;
        public T Value
        {
            get => m_value;
            set
            {
                if (!Comparer.Equals(value, m_value))
                {
                    m_value = value;
                    OnValueChanged?.Invoke(m_value);
                }
            }
        }

        event Action<T> OnValueChanged;

        public BindableProperty(T value = default)
        {
            m_value = value;
        }

        public void SetValueSilently(T value)
        {
            m_value = value;
        }

        public IUnRegister Register(Action<T> onValueChanged)
        {
            OnValueChanged += onValueChanged;
            return new UnRegister<Action<T>>(this, onValueChanged);
        }

        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(m_value);
            return Register(onValueChanged);
        }

        public void UnRegister(Action<T> onValueChanged)
        {
            OnValueChanged -= onValueChanged;
        }

        public static implicit operator T (BindableProperty<T> property)
        {
            return property.m_value;
        }
    }
}