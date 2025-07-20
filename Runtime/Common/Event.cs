using System;

namespace GameFramework
{
    public interface IEvent
    {
        
    }

    public class Event : IEvent, ICanceller<Action>
    {
        private event Action OnEvent;

        public ICancelToken Register(Action onEvent)
        {
            OnEvent += onEvent;
            return new CancelToken<Action>(this, onEvent);
        }

        public void Cancel(Action onEvent)
        {
            OnEvent -= onEvent;
        }

        public void Invoke()
        {
            OnEvent?.Invoke();
        }
    }

    public class Event<T> : IEvent, ICanceller<Action<T>>
    {
        private event Action<T> OnEvent;
        
        public ICancelToken Register(Action<T> onEvent)
        {
            OnEvent += onEvent;
            return new CancelToken<Action<T>>(this, onEvent);
        }

        public void Cancel(Action<T> onEvent)
        {
            OnEvent -= onEvent;
        }

        public void Invoke(in T e)
        {
            OnEvent?.Invoke(e);
        }
    }
}
