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
        private Action<T> mOnEvent;

        public EventSystemUnRegister(IEventSystem eventSystem, Action<T> onEvent)
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
        void Send<T>() where T : new();
        void Send<T>(in T e);

        IUnRegister Register<T>(Action<T> onEvent);
        void UnRegister<T>(Action<T> onEvent);
    }

    public class EventSystem : IEventSystem
    {
        public interface IRegistrations
        {
        }

        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent;
        }

        private Dictionary<Type, IRegistrations> m_EventRegistration = new Dictionary<Type, IRegistrations>();

        public void Send<T>() where T : new()
        {
            var e = new T();
            Send(e);
        }

        public void Send<T>(in T e)
        {
            var type = typeof(T);
            if (m_EventRegistration.TryGetValue(type, out var registrations))
            {
                (registrations as Registrations<T>)?.OnEvent(e);
            }
        }

        public IUnRegister Register<T>(Action<T> onEvent)
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

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            if (m_EventRegistration.TryGetValue(type, out var registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
}