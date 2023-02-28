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

    public interface IBindableProperty<T> : IReadonlyBindableProperty<T>
    {
        new T Value { get; set; }

        void SetValueSilently(T value);
    }

    public class BindableProperty<T> : IBindableProperty<T>, IUnRegisterable<Action<T>>
    {
        private static readonly IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

        private event Action<T> OnValueChanged;

        private T m_value = default;
        public T Value
        {
            get => GetValue();
            set
            {
                if (!comparer.Equals(value, m_value))
                {
                    SetValue(value);
                    OnValueChanged?.Invoke(m_value);
                }
            }
        }

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
            return new UnRegisterableUnRegister<Action<T>>(this, onValueChanged);
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

        protected virtual T GetValue()
        {
            return m_value;
        }

        protected virtual void SetValue(T value)
        {
            m_value = value;
        }

        public static implicit operator T (BindableProperty<T> property)
        {
            return property.Value;
        }
    }
}