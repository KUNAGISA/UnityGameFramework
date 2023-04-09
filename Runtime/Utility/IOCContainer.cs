using System;
using System.Collections.Generic;
using System.Linq;
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

            var type = instance.GetType();
            foreach(var fieldInfo in type.GetRuntimeFields())
            {
                if (fieldInfo.GetCustomAttribute<InjectAttribute>(true) != null)
                {
                    fieldInfo.SetValue(instance, GetOfType(fieldInfo.FieldType));
                }
            }
            foreach (var propertyInfo in type.GetRuntimeProperties())
            {
                if (propertyInfo.GetCustomAttribute<InjectAttribute>(true) != null)
                {
                    propertyInfo.SetValue(instance, GetOfType(propertyInfo.PropertyType));
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
            if (m_instances.TryGetValue(typeof(T), out var instance))
            {
                return (T)instance;
            }
            return m_instances.Values.OfType<T>().FirstOrDefault();
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
            return m_instances.Values.OfType<T>();
        }

        public void Clear()
        {
            m_instances.Clear();
        }
    }
}