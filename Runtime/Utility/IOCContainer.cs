using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class InjectAttribute : Attribute
    {

    }

    public interface IIOCContainer
    {
        void Register<T>(T instance, bool injectNow = false) where T : class;
        void UnRegister<T>();
        bool UnRegister<T>(out T instance);
        void Inject(object instance);
        T Get<T>();
        T GetOfType<T>();
        bool TryGet<T>(out T instance);
        bool TryGetOfType<T>(out T instance);
        IEnumerable<T> EachOfType<T>();
        void Clear();
    }

    public sealed class IOCContainer : IIOCContainer
    {
        private readonly Dictionary<Type, object> m_instances = new Dictionary<Type, object>();

        public void Register<T>(T instance, bool injectNow = false) where T : class
        {
            var key = typeof(T);
            if (!m_instances.TryAdd(key, instance))
            {
                m_instances[key] = instance;
            }
            if (injectNow)
            {
                Inject(instance);
            }
        }

        public void UnRegister<T>()
        {
            m_instances.Remove(typeof(T));
        }

        public bool UnRegister<T>(out T instance)
        {
            if (m_instances.TryGetValue(typeof(T), out var value))
            {
                instance = (T)value;
                return true;
            }

            instance = default;
            return false;
        }

        public void Inject(object instance)
        {
            if (instance == null)
            {
                return;
            }

            var members = instance.GetType().GetMembers();
            foreach (var memberInfo in members)
            {
                var injectAttribute = memberInfo.GetCustomAttribute<InjectAttribute>(true);
                if (injectAttribute != null)
                {
                    if (memberInfo is PropertyInfo propertyInfo)
                    {
                        propertyInfo.SetValue(instance, GetOfType(propertyInfo.PropertyType), null);
                    }
                    else if (memberInfo is FieldInfo fieldInfo)
                    {
                        fieldInfo.SetValue(instance, GetOfType(fieldInfo.FieldType));
                    }
                }
            }
        }

        public T Get<T>()
        {
            return m_instances.TryGetValue(typeof(T), out var instance) ? ((T)instance) : default;
        }

        public object Get(Type type)
        {
            return m_instances.TryGetValue(type, out var instance) ? instance : null;
        }

        public T GetOfType<T>()
        {
            var target = typeof(T);
            if (m_instances.TryGetValue(target, out var instance))
            {
                return (T)instance;
            }

            foreach(var (type, value) in m_instances)
            {
                if (target.IsAssignableFrom(type))
                {
                    return (T)value;
                }
            }

            return default;
        }

        public object GetOfType(Type type)
        {
            if (m_instances.TryGetValue(type, out var @object))
            {
                return @object;
            }
            foreach(var instance in m_instances.Values)
            {
                if (type.IsAssignableFrom(instance.GetType()))
                {
                    return instance;
                }
            }
            return null;
        }

        public bool TryGet<T>(out T result)
        {
            result = Get<T>();
            return result != null;
        }

        public bool TryGet(Type type, out object result)
        {
            result = Get(type);
            return result != null;
        }

        public bool TryGetOfType<T>(out T result)
        {
            result = GetOfType<T>();
            return result != null;
        }

        public bool TryGetOfType(Type type, out object result)
        {
            result = GetOfType(type);
            return result != null;
        }

        public IEnumerable<T> EachOfType<T>()
        {
            var target = typeof(T);
            foreach (var (type, value) in m_instances)
            {
                if (target.IsAssignableFrom(type))
                {
                    yield return (T)value;
                }
            }
        }

        public void Clear()
        {
            m_instances.Clear();
        }
    }
}