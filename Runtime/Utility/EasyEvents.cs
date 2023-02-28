using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IEasyEvent
    { 

    }

    public class EasyEvent : IEasyEvent, IUnRegisterable<Action>
    {
        private event Action OnEvent;

        public IUnRegister Register(Action onEvent)
        {
            OnEvent += onEvent;
            return new UnRegisterableUnRegister<Action>(this, onEvent);
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

    public class EasyEvent<T> : IEasyEvent, IUnRegisterable<Action<T>>
    {
        private event Action<T> OnEvent;

        public IUnRegister Register(Action<T> onEvent)
        {
            OnEvent += onEvent;
            return new UnRegisterableUnRegister<Action<T>>(this, onEvent);
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
        private readonly Dictionary<Type, IEasyEvent> m_typeEvents = new Dictionary<Type, IEasyEvent>();

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
