using System;

namespace Framework
{
    public static class Shared<T> where T : class, new()
    {
        public struct SharedScope : IDisposable
        {
            private bool m_disposed;
            public static SharedScope Create()
            {
                return new SharedScope() { m_disposed = false };
            }

            public void Dispose()
            {
                if (!m_disposed) sharing = false;
                m_disposed = true;
            }
        }

        private static bool sharing = false;
        private static T instance = null;

        public static SharedScope Get(out T shared)
        {
            System.Diagnostics.Debug.Assert(!sharing, $"Shared<{typeof(T)}> is using.");
            shared = instance ??= new T();
            sharing = true;
            return SharedScope.Create();
        }

        public static void Clear()
        {
            System.Diagnostics.Debug.Assert(!sharing, $"Shared<{typeof(T)}> is using.");
            instance = null;
        }
    }
}