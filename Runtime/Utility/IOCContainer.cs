using System;
using System.Collections.Generic;

namespace Framework
{
    public class IOCContainer
    {
        private readonly Dictionary<Type, object> m_instances = new Dictionary<Type, object>();

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

        public bool TryGet<T>(out T @object) where T : class
        {
            @object = null;
            if (m_instances.TryGetValue(typeof(T), out var instance))
            {
                @object = (T)instance;
            }
            return @object != null;
        }

        public IEnumerable<T> Select<T>() where T : class
        {
            foreach (var (_, @object) in m_instances)
            {
                if (@object is T instance)
                {
                    yield return instance;
                }
            }
        }
    }
}
