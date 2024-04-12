using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IEasyEvent
    { 

    }

    public class EasyEvent : IEasyEvent, IDeregister<Action>
    {
        public event Action OnEvent;

        public IUnRegister Register(Action onEvent)
        {
            OnEvent += onEvent;
            return new DeregisterUnRegister<Action>(this, onEvent);
        }

        public void UnRegister(Action onEvent)
        {
            OnEvent -= onEvent;
        }

        public void Trigger()
        {
            OnEvent?.Invoke();
        }
    }

    public class EasyEvent<T> : IEasyEvent, IDeregister<Action<T>>
    {
        public event Action<T> OnEvent;

        public IUnRegister Register(Action<T> onEvent)
        {
            OnEvent += onEvent;
            return new DeregisterUnRegister<Action<T>>(this, onEvent);
        }

        public void UnRegister(Action<T> onEvent)
        {
            OnEvent -= onEvent;
        }

        public void Trigger(T @event)
        {
            OnEvent?.Invoke(@event);
        }
    }

    public class EasyEvents
    {
        private readonly Dictionary<Type, IEasyEvent> m_events = new Dictionary<Type, IEasyEvent>();

        public void AddEvent<T>() where T : IEasyEvent, new()
        {
            m_events.Add(typeof(T), new T());
        }

        public T GetEvent<T>() where T : IEasyEvent
        {
            if (m_events.TryGetValue(typeof(T), out var easyEvent))
            {
                return (T)easyEvent;
            }
            return default;
        }

        public T GetOrAddEvent<T>() where T : IEasyEvent, new()
        {
            var type = typeof(T);
            if (!m_events.TryGetValue(type, out var easyEvent))
            {
                easyEvent = new T();
                m_events.Add(type, easyEvent);
            }
            return (T)easyEvent;
        }

        public void Clear()
        {
            m_events.Clear();
        }
    }
}
