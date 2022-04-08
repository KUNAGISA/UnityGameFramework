using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class IOCContainer : IEnumerable<object>
    {
        readonly Dictionary<Type, object> m_Instances = new Dictionary<Type, object>();

        public void Register<T>(T instance) where T : class
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

        public void UnRegister<T>() where T : class
        {
            m_Instances.Remove(typeof(T));
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

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            foreach(var value in m_Instances.Values)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var value in m_Instances.Values)
            {
                yield return value;
            }
        }
    }
}