using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework
{
    public sealed class IOCContainer
    {
        private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        public bool Contains<T>()
        {
            return _instances.ContainsKey(typeof(T));
        }

        public void Register<T>(T instance)
        {
            var key = typeof(T);
            _instances[key] = instance;
        }

        public bool UnRegister<T>(out T instance) where T : class
        {
            instance = null;
            if (_instances.Remove(typeof(T), out var value))
            {
                instance = (T)value;
            }
            return instance != null;
        }

        public T Get<T>() where T : class
        {
            return _instances.TryGetValue(typeof(T), out var instance) ? (T)instance : null;
        }

        public IEnumerable<T> Select<T>() where T : class
        {
            return _instances.Values.OfType<T>();
        }

        public void Clear()
        {
            _instances.Clear();
        }
    }
}
