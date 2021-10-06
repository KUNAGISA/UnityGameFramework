using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class IOCContainer
    {
        Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = typeof(T);
            if (!mInstances.ContainsKey(key))
            {
                mInstances.Add(key, instance);
            }
            else
            {
                mInstances[key] = instance;
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);
            if (mInstances.TryGetValue(key, out var instance))
            {
                return instance as T;
            }
            return null;
        }
    }
}