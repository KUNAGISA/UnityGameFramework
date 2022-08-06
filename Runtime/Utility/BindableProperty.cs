using System;

namespace Framework
{
    public class BindableProperty<T> where T : IEquatable<T>
    {
        private event Action<T> m_onValueChanged;

        private T m_value = default(T);
        public T value
        {
            get => m_value;

            set
            {
                if (!value.Equals(m_value))
                {
                    m_value = value;
                    m_onValueChanged?.Invoke(m_value);
                }

            }
        }

        public BindableProperty(T value = default(T)) => m_value = value;

        /// <summary>
        /// 注册回调
        /// </summary>
        /// <param name="onValueChanged">值更变回调</param>
        /// <returns>注销句柄</returns>
        public IUnRegister Register(Action<T> onValueChanged)
        {
            m_onValueChanged += onValueChanged;
            return new CustomUnRegister<Action<T>>(UnRegister, onValueChanged);
        }

        /// <summary>
        /// 注册回调并初始化
        /// </summary>
        /// <param name="onValueChanged">值更变回调</param>
        /// <returns>注销句柄</returns>
        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(m_value);
            return Register(onValueChanged);
        }

        /// <summary>
        /// 注销回调
        /// </summary>
        /// <param name="onValueChanged">回调</param>
        public void UnRegister(Action<T> onValueChanged)
        {
            m_onValueChanged -= onValueChanged;
        }

        public static implicit operator T (BindableProperty<T> property) => property.m_value;

        public static implicit operator BindableProperty<T> (T value) => new BindableProperty<T>(value);
    }
}