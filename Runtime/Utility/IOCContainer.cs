using System;
using System.Collections.Generic;

namespace Framework
{
    public class IOCContainer
    {
        readonly Dictionary<Type, object> m_instances = new Dictionary<Type, object>();

        public void Register<T>(T instance) where T : class
        {
            var key = typeof(T);
            if (!m_instances.ContainsKey(key))
            {
                m_instances.Add(key, instance);
            }
            else
            {
                m_instances[key] = instance;
            }
        }

        public void UnRegister<T>() where T : class
        {
            m_instances.Remove(typeof(T));
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);
            if (m_instances.TryGetValue(key, out var instance))
            {
                return instance as T;
            }
            return null;
        }

        public void Each(Action<Type, object> handler)
        {
            foreach(var pairs in m_instances)
            {
                handler(pairs.Key, pairs.Value);
            }
        }

        public void Each<T>(Action<T> handler) where T : class
        {
            foreach (var pairs in m_instances)
            {
                var instance = pairs.Value as T;
                if (instance != null)
                {
                    handler(instance);
                }
            }
        }
    }
}