using System;

namespace Framework
{
    public sealed class BindableValue<T> : IReadonlyBindableProperty<T>, IBindableProperty<T>, IUnRegisterable<Action<T>> where T : struct, IEquatable<T>
    {
        private T m_value = default;
        public T Value
        {
            get => m_value;
            set
            {
                if (!m_value.Equals(value))
                {
                    m_value = value;
                    OnValueChanged?.Invoke(m_value);
                }
            }
        }

        public event Action<T> OnValueChanged;

        public BindableValue()
        {

        }

        public BindableValue(T value) 
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

        public void SetValueSilently(T value)
        {
            m_value = value;
        }

        public static implicit operator T(BindableValue<T> property)
        {
            return property.m_value;
        }
    }
}