using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    public sealed class IOCContainer
    {
        private readonly Dictionary<Type, object> m_instances = new Dictionary<Type, object>();

        public bool Contains<T>()
        {
            return m_instances.ContainsKey(typeof(T));
        }

        public void Register<T>(T instance)
        {
            var key = typeof(T);

            if (m_instances.ContainsKey(key))
            {
                m_instances[key] = instance;
            }
            else
            {
                m_instances.Add(key, instance);
            }
        }

        public bool UnRegister<T>(out T instance) where T : class
        {
            instance = null;
            if (m_instances.Remove(typeof(T), out var @object))
            {
                instance = (T)@object;
            }
            return instance != null;
        }

        public T Get<T>() where T : class
        {
            return m_instances.TryGetValue(typeof(T), out var instance) ? (T)instance : null;
        }

        public IEnumerable<T> Select<T>() where T : class
        {
            return m_instances.Values.OfType<T>();
        }

        public void Clear()
        {
            m_instances.Clear();
        }
    }
}
