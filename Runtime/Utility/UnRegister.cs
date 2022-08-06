using System;

namespace Framework
{
    public interface IUnRegister
    {
        void UnRegister();
    }

    public sealed class CustomUnRegister<T> : IUnRegister where T : class
    {
        private Action<T> m_onUnRegister = null;
        private T m_unRegister = null;

        public CustomUnRegister(Action<T> onUnRegister, T unRegister)
        {
            m_onUnRegister = onUnRegister;
            m_unRegister = unRegister;
        }

        public void UnRegister()
        {
            m_onUnRegister?.Invoke(m_unRegister);
            m_onUnRegister = null;
            m_unRegister = null;
        }
    }
}
