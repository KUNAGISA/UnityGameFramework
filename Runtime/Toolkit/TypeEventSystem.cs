using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed class TypeEventSystem
    {
        private readonly Dictionary<Type, IEasyEvent> m_events = new Dictionary<Type, IEasyEvent>();

        public void Send<T>(in T e)
        {
            if (m_events.TryGetValue(typeof(EasyEvent<T>), out var trigger))
            {
                ((EasyEvent<T>)trigger).Trigger(e);
            }
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            if (!m_events.TryGetValue(typeof(EasyEvent<T>), out var trigger))
            {
                trigger = new EasyEvent<T>();
                m_events.Add(typeof(EasyEvent<T>), trigger);
            }
            return ((EasyEvent<T>)trigger).Register(onEvent);
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            if (m_events.TryGetValue(typeof(EasyEvent<T>), out var trigger))
            {
                ((EasyEvent<T>)trigger).UnRegister(onEvent);
            }
        }

        public void Clear()
        {
            m_events.Clear();
        }
    }
}