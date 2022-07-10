using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IUnRegister
    {
        void UnRegister();
    }

    public struct EventSystemUnRegister<T> : IUnRegister
    {
        private readonly WeakReference<IEventSystem> m_eventSystem;
        private IEventSystem.OnEventHandler<T> m_onEvent;

        public EventSystemUnRegister(IEventSystem eventSystem, IEventSystem.OnEventHandler<T> onEvent)
        {
            m_onEvent = onEvent;
            m_eventSystem = new WeakReference<IEventSystem>(eventSystem);
        }

        public void UnRegister()
        {
            if (m_onEvent != null && m_eventSystem.TryGetTarget(out var eventSystem))
            {
                eventSystem.UnRegister(m_onEvent);
            }
            m_eventSystem.SetTarget(null);
            m_onEvent = null;
        }
    }

    public interface IEventSystem
    {
        delegate void OnEventHandler<T>(in T @event);

        void Send<T>() where T : new();
        void Send<T>(in T @event);

        IUnRegister Register<T>(OnEventHandler<T> onEvent);
        void UnRegister<T>(OnEventHandler<T> onEvent);
    }

    public class EventSystem : IEventSystem
    {
        public interface IRegistrations
        {
        }

        public class Registrations<T> : IRegistrations
        {
            public IEventSystem.OnEventHandler<T> onEvent;
        }

        private readonly Dictionary<Type, IRegistrations> m_eventRegistration = new Dictionary<Type, IRegistrations>();

        public void Send<T>() where T : new()
        {
            var @event = new T();
            Send(in @event);
        }

        public void Send<T>(in T @event)
        {
            var type = typeof(T);
            if (m_eventRegistration.TryGetValue(type, out var registrations))
            {
                (registrations as Registrations<T>)?.onEvent(in @event);
            }
        }

        public IUnRegister Register<T>(IEventSystem.OnEventHandler<T> onEvent)
        {
            var type = typeof(T);
            if (!m_eventRegistration.TryGetValue(type, out var registrations))
            {
                registrations = new Registrations<T>();
                m_eventRegistration.Add(type, registrations);
            }

            (registrations as Registrations<T>).onEvent += onEvent;

            return new EventSystemUnRegister<T>(this, onEvent);
        }

        public void UnRegister<T>(IEventSystem.OnEventHandler<T> onEvent)
        {
            var type = typeof(T);
            if (m_eventRegistration.TryGetValue(type, out var registrations))
            {
                (registrations as Registrations<T>).onEvent -= onEvent;
            }
        }
    }
}