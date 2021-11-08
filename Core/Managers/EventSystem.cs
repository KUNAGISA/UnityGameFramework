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
        private readonly WeakReference<IEventSystem> m_EventSystem;
        private IEventSystem.OnEventHandler<T> m_OnEvent;

        public EventSystemUnRegister(IEventSystem eventSystem, IEventSystem.OnEventHandler<T> onEvent)
        {
            m_OnEvent = onEvent;
            m_EventSystem = new WeakReference<IEventSystem>(eventSystem);
        }

        public void UnRegister()
        {
            if (m_OnEvent != null && m_EventSystem.TryGetTarget(out var eventSystem))
            {
                eventSystem.UnRegister(m_OnEvent);
            }
            m_EventSystem.SetTarget(null);
            m_OnEvent = null;
        }
    }

    public interface IEventSystem
    {
        delegate void OnEventHandler<T>(in T e);

        void Send<T>() where T : new();
        void Send<T>(in T e);

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
            public IEventSystem.OnEventHandler<T> OnEvent;
        }

        private readonly Dictionary<Type, IRegistrations> m_EventRegistration = new Dictionary<Type, IRegistrations>();

        public void Send<T>() where T : new()
        {
            var e = new T();
            Send(in e);
        }

        public void Send<T>(in T e)
        {
            var type = typeof(T);
            if (m_EventRegistration.TryGetValue(type, out var registrations))
            {
                (registrations as Registrations<T>)?.OnEvent(in e);
            }
        }

        public IUnRegister Register<T>(IEventSystem.OnEventHandler<T> onEvent)
        {
            var type = typeof(T);
            if (!m_EventRegistration.TryGetValue(type, out var registrations))
            {
                registrations = new Registrations<T>();
                m_EventRegistration.Add(type, registrations);
            }

            (registrations as Registrations<T>).OnEvent += onEvent;

            return new EventSystemUnRegister<T>(this, onEvent);
        }

        public void UnRegister<T>(IEventSystem.OnEventHandler<T> onEvent)
        {
            var type = typeof(T);
            if (m_EventRegistration.TryGetValue(type, out var registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
}