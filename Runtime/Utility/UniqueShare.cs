using System;

namespace Framework
{
    public struct UniqueShare<T> : IDisposable where T : class, new()
    {
        private static T shareInstance = null;

        public static void Clearup()
        {
            shareInstance = null;
        }

        private T m_keepInstance;

        public UniqueShare(out T value)
        {
            m_keepInstance = value = shareInstance ?? new T();
            shareInstance = null;
        }

        public void Dispose()
        {
            shareInstance = m_keepInstance ?? shareInstance;
            m_keepInstance = null;
        }
    }
}