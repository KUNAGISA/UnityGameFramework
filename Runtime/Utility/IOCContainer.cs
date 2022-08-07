using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    { 
    }

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

        public void Inject(object instance)
        {
            var memberInfos = instance.GetType().GetTypeInfo().DeclaredMembers;
            foreach(var memberInfo in memberInfos)
            {
                if (memberInfo.GetCustomAttribute<InjectAttribute>() == null)
                {
                    continue;
                }
                if (memberInfo.MemberType == MemberTypes.Property)
                {
                    var propertyInfo = memberInfo as PropertyInfo;
                    if (m_instances.TryGetValue(propertyInfo.PropertyType, out var value))
                    {
                        propertyInfo.SetValue(instance, value);
                    }
                    
                }
                else if (memberInfo.MemberType == MemberTypes.Field)
                {
                    var feildInfo = memberInfo as FieldInfo;
                    if (m_instances.TryGetValue(feildInfo.FieldType, out var value))
                    {
                        feildInfo.SetValue(instance, value);
                    }
                }
            }
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