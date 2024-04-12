using System;

namespace Framework
{
    public interface IUnRegister
    {
        void UnRegister();
    }

    public interface IDeregister<T>
    {
        void UnRegister(T register);
    }

    public sealed class CustomUnRegister : IUnRegister
    {
        private Action m_deregister = null;

        public CustomUnRegister(Action deregister)
        {
            m_deregister = deregister;
        }

        public void UnRegister()
        {
            m_deregister?.Invoke();
            m_deregister = null;
        }
    }

    public sealed class DeregisterUnRegister<T> : IUnRegister
    {
        private IDeregister<T> m_deregister = null;
        private T m_register = default;

        public DeregisterUnRegister(IDeregister<T> deregister, T register)
        {
            m_deregister = deregister;
            m_register = register;
        }

        public void UnRegister()
        {
            m_deregister?.UnRegister(m_register);
            m_deregister = null; m_register = default;
        }
    }
}
