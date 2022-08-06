using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IEventSystem
    {
        delegate void OnEventHandler<T>(in T @event) where T : struct;

        void Send<T>() where T : struct;

        void Send<T>(in T @event) where T : struct;

        IUnRegister Register<T>(OnEventHandler<T> onEvent) where T : struct;

        void UnRegister<T>(OnEventHandler<T> onEvent) where T : struct;
    }

    public class EventSystem : IEventSystem
    {
        private interface IRegisterEvent { }

        private class RegisterEvent<T> : IRegisterEvent where T : struct
        {
            public event IEventSystem.OnEventHandler<T> onEvent;

            public void OnEvent(in T @event) => onEvent?.Invoke(in @event);
        }

        private readonly Dictionary<Type, IRegisterEvent> m_registerEventMap = new Dictionary<Type, IRegisterEvent>();

        public void Send<T>() where T : struct
        {
            var @event = new T();
            Send(in @event);
        }

        public void Send<T>(in T @event) where T : struct
        {
            var type = typeof(T);
            if (m_registerEventMap.TryGetValue(type, out var registrations))
            {
                (registrations as RegisterEvent<T>)?.OnEvent(in @event);
            }
        }

        public IUnRegister Register<T>(IEventSystem.OnEventHandler<T> onEvent) where T : struct
        {
            var type = typeof(T);
            if (!m_registerEventMap.TryGetValue(type, out var registrations))
            {
                registrations = new RegisterEvent<T>();
                m_registerEventMap.Add(type, registrations);
            }
            (registrations as RegisterEvent<T>).onEvent += onEvent;

            return new CustomUnRegister<IEventSystem.OnEventHandler<T>>(UnRegister, onEvent);
        }

        public void UnRegister<T>(IEventSystem.OnEventHandler<T> onEvent) where T : struct
        {
            var type = typeof(T);
            if (m_registerEventMap.TryGetValue(type, out var registrations))
            {
                (registrations as RegisterEvent<T>).onEvent -= onEvent;
            }
        }
    }
}