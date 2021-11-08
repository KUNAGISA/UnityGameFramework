using System;
using System.Collections.Generic;

namespace Framework
{
    public class IOCContainer
    {
        readonly Dictionary<Type, object> m_Instances = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = typeof(T);
            if (!m_Instances.ContainsKey(key))
            {
                m_Instances.Add(key, instance);
            }
            else
            {
                m_Instances[key] = instance;
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);
            if (m_Instances.TryGetValue(key, out var instance))
            {
                return instance as T;
            }
            return null;
        }
    }
}