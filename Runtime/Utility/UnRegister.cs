using System;

namespace Framework
{
    public interface IUnRegister
    {
        void UnRegister();
    }

    public interface IUnRegisterable<T>
    {
        void UnRegister(T register);
    }

    public sealed class UnRegisterableUnRegister<T> : IUnRegister where T : class
    {
        private IUnRegisterable<T> m_unregisterable = null;
        private T m_register = null;

        public UnRegisterableUnRegister(IUnRegisterable<T> unregisterable, T register)
        {
            m_unregisterable = unregisterable;
            m_register = register;
        }

        public void UnRegister()
        {
            m_unregisterable?.UnRegister(m_register);
            m_unregisterable = null; m_register = null;
        }
    }
}
