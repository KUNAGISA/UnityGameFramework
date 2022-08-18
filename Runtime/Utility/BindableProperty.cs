using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IReadonlyBindableProperty<T>
    {
        T value { get; }

        IUnRegister Register(Action<T> onValueChanged);

        IUnRegister RegisterWithInitValue(Action<T> onValueChanged);

        void UnRegister(Action<T> onValueChanged);
    }

    public interface IBindableProperty<T> : IReadonlyBindableProperty<T>
    {
        new T value { get; set; }

        void SetValueSilently(T value);
    }

    public class BindableProperty<T> : IBindableProperty<T>
    {
        private static readonly IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

        private event Action<T> m_onValueChanged;

        private T m_value = default;
        public T value
        {
            get => GetValue();
            set
            {
                if (!comparer.Equals(value, m_value))
                {
                    SetValue(value);
                    m_onValueChanged?.Invoke(m_value);
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
            m_onValueChanged += onValueChanged;
            return new CustomUnRegister<Action<T>>(UnRegister, onValueChanged);
        }

        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(m_value);
            return Register(onValueChanged);
        }

        public void UnRegister(Action<T> onValueChanged)
        {
            m_onValueChanged -= onValueChanged;
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
            return property.value;
        }
    }
}