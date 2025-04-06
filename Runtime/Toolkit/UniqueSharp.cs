using System;

namespace Framework
{
    public interface IRecyclable
    {
        protected internal void OnRecycle();
    }

    public struct UniqueSharpScope : IDisposable
    {
        private object m_target;
        private Action<object> m_onRecycle;

        internal UniqueSharpScope(object target, Action<object> onRecycle)
        {
            m_target = target;
            m_onRecycle = onRecycle;
        }

        void IDisposable.Dispose()
        {
            m_onRecycle?.Invoke(m_target);
            m_target = null; m_onRecycle = null;
        }
    }

    public static class UniqueSharp<T> where T : class, new()
    {
        private static T s_sharp = null;

        public static T Get()
        {
            var sharp = s_sharp ?? new T();
            s_sharp = null;
            return sharp;
        }

        public static void Recycle(T sharp)
        {
            s_sharp = sharp;
            (s_sharp as IRecyclable)?.OnRecycle();
        }

        public static UniqueSharpScope Get(out T sharp)
        {
            sharp = s_sharp ?? new();
            s_sharp = null;
            return new UniqueSharpScope(sharp, Recycle);
        }

        private static void Recycle(object sharp)
        {
            s_sharp = (T)sharp;
            (s_sharp as IRecyclable)?.OnRecycle();
        }
    }
}
