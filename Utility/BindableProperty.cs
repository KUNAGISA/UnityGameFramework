using System;

namespace Framework
{

    public class BindableProperty<T> where T : IEquatable<T>
    {
        public delegate void OnBindablePropertyChange(in T value);
        public OnBindablePropertyChange OnValueChanged;

        private T mValue = default(T);

        public T Value
        {
            get => mValue;

            set
            {
                if (!value.Equals(mValue))
                {
                    mValue = value;
                    OnValueChanged?.Invoke(in mValue);
                }

            }
        }
    }
}