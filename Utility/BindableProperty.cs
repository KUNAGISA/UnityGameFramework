using System;

namespace Framework
{

    public class BindableProperty<T> where T : IEquatable<T>
    {
        public delegate void OnBindablePropertyChange(in T value);
        public OnBindablePropertyChange OnValueChanged;

        private T m_Value = default;

        public T Value
        {
            get => m_Value;

            set
            {
                if (!value.Equals(m_Value))
                {
                    m_Value = value;
                    OnValueChanged?.Invoke(in m_Value);
                }

            }
        }
    }
}