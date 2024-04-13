using System;

namespace Framework
{
    public class TypeEventSystem
    {
        private readonly EasyEvents m_events = new EasyEvents();

        public void Send<T>() where T : new()
        {
            m_events.GetEvent<EasyEvent<T>>()?.Trigger(new T());
        }

        public void Send<T>(T @event)
        {
            m_events.GetEvent<EasyEvent<T>>()?.Trigger(@event);
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            return m_events.GetOrAddEvent<EasyEvent<T>>().Register(onEvent);
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            m_events.GetEvent<EasyEvent<T>>()?.UnRegister(onEvent);
        }

        public void Clear()
        {
            m_events.Clear();
        }
    }
}