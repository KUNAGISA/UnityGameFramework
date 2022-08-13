using System;

namespace Framework
{
    public class BindableRefProperty<T> where T : class
    {
        private event Action<T> m_onValueChanged;

        private T m_value = null;
        public T value
        {
            get => m_value;

            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    m_onValueChanged?.Invoke(m_value);
                }
            }
        }

        public BindableRefProperty(T value = null) => m_value = value;

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

        public static implicit operator T (BindableRefProperty<T> property) => property.m_value;
    }

    public class BindableProperty<T> where T : struct, IEquatable<T>
    {
        private event Action<T> m_onValueChanged;

        private T m_value = default(T);
        public T value
        {
            get => m_value;

            set
            {
                if (!m_value.Equals(value))
                {
                    m_value = value;
                    m_onValueChanged?.Invoke(m_value);
                }
            }
        }

        public BindableProperty(T value = default(T)) => m_value = value;

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

        public static implicit operator T (BindableProperty<T> property) => property.m_value;
    }
}