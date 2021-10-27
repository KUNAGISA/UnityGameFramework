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
        private IEventSystem mEventSystem;
        private IEventSystem.OnEventHandler<T> mOnEvent;

        public EventSystemUnRegister(IEventSystem eventSystem, IEventSystem.OnEventHandler<T> onEvent)
        {
            mEventSystem = eventSystem;
            mOnEvent = onEvent;
        }

        public void UnRegister()
        {
            mEventSystem.UnRegister(mOnEvent);
            mEventSystem = null;
            mOnEvent = null;
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

        private Dictionary<Type, IRegistrations> m_EventRegistration = new Dictionary<Type, IRegistrations>();

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