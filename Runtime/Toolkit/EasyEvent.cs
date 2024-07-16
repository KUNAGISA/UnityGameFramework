using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IEasyEvent
    {
        IUnRegister Register(Action onEvent);
    }

    public class EasyEvent : IEasyEvent, IUnRegisterable<Action>
    {
        public event Action OnEvent;

        public IUnRegister Register(Action onEvent)
        {
            OnEvent += onEvent;
            return new UnRegister<Action>(this, onEvent);
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
        public event Action<T> OnEvent;

        public IUnRegister Register(Action onEvent)
        {
            return Register(x => onEvent());
        }

        public IUnRegister Register(Action<T> onEvent)
        {
            OnEvent += onEvent;
            return new UnRegister<Action<T>>(this, onEvent);
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
}
