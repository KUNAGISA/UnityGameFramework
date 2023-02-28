using System;
using System.Collections.Generic;

namespace Framework
{
    public class IOCContainer
    {
        private readonly Dictionary<Type, object> m_instances = new Dictionary<Type, object>();

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
            var type = typeof(T);
            if (m_instances.TryGetValue(type, out var instance))
            {
                return instance as T;
            }
            return null;
        }

        public bool TryGet<T>(out T result) where T : class
        {
            result = Get<T>();
            return result != null;
        }

        public void ForEach<T>(Action<T> action)
        {
            foreach(var (_, instance) in m_instances)
            {
                if (instance is T target)
                {
                    action(target);
                }
            }
        }
    }
}