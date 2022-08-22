using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IEasyEvent
    { 
    }

    public class EasyEvent : IEasyEvent
    {
        private event Action m_onEvent;

        public IUnRegister Register(Action onEvent)
        {
            m_onEvent += onEvent;
            return new CustomUnRegister<Action>(UnRegister, onEvent);
        }

        public void UnRegister(Action onEvent)
        {
            m_onEvent -= onEvent;
        }

        public void Trigger()
        {
            m_onEvent();
        }
    }

    public class EasyEvent<T>
    {
        private event Action<T> m_onEvent;

        public IUnRegister Register(Action<T> onEvent)
        {
            m_onEvent += onEvent;
            return new CustomUnRegister<Action<T>>(UnRegister, onEvent);
        }

        public void UnRegister(Action<T> onEvent)
        {
            m_onEvent -= onEvent;
        }

        public void Trigger(T @event)
        {
            m_onEvent(@event);
        }
    }

    public class EasyValueEvent<T> : IEasyEvent where T : struct
    {
        private event ValueAction<T> m_onEvent;

        public IUnRegister Register(ValueAction<T> onEvent)
        {
            m_onEvent += onEvent;
            return new CustomUnRegister<ValueAction<T>>(UnRegister, onEvent);
        }

        public void UnRegister(ValueAction<T> onEvent)
        {
            m_onEvent -= onEvent;
        }

        public void Trigger(in T @event)
        {
            m_onEvent(@event);
        }
    }

    public class EasyEvents
    {
        private Dictionary<Type, IEasyEvent> m_typeEvents = new Dictionary<Type, IEasyEvent>();

        public void AddEvent<T>() where T : IEasyEvent, new()
        {
            m_typeEvents.Add(typeof(T), new T());
        }

        public T GetEvent<T>() where T : IEasyEvent
        {
            if (m_typeEvents.TryGetValue(typeof(T), out var easyEvent))
            {
                return (T)easyEvent;
            }
            return default;
        }

        public T GetOrAddEvent<T>() where T : IEasyEvent, new()
        {
            var type = typeof(T);
            if (!m_typeEvents.TryGetValue(type, out var easyEvent))
            {
                easyEvent = new T();
                m_typeEvents.Add(type, easyEvent);
            }
            return (T)easyEvent;
        }
    }
}
