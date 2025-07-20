using System;
using System.Collections.Generic;

namespace GameFramework
{
    public sealed class TypedEvent
    {
        private readonly Dictionary<Type, IEvent> _events = new Dictionary<Type, IEvent>();

        public void Send<T>(in T e)
        {
            if (_events.TryGetValue(typeof(Event<T>), out var trigger))
            {
                ((Event<T>)trigger).Invoke(e);
            }
        }

        public ICancelToken Register<T>(Action<T> onEvent)
        {
            var key = typeof(T);
            if (_events.TryGetValue(key, out var trigger))
            {
                return ((Event<T>)trigger).Register(onEvent);
            }
            trigger = new Event<T>();
            _events.Add(key, trigger);
            return ((Event<T>)trigger).Register(onEvent);
        }

        public void Cancel<T>(Action<T> onEvent)
        {
            if (_events.TryGetValue(typeof(Event<T>), out var trigger))
            {
                ((Event<T>)trigger).Cancel(onEvent);
            }
        }

        public void Clear()
        {
            _events.Clear();
        }
    }
}