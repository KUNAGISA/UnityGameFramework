using System;

namespace GameFramework
{
    public interface IEventChannel
    {
        
    }

    public class EventChannel<T> : IEventChannel, ICanceller<Action<T>>
    {
        private event Action<T> _callback;
        
        public ICancelToken Register(Action<T> callback)
        {
            _callback += callback;
            return CancelToken.Get(this, callback);
        }

        public void Cancel(Action<T> callback)
        {
            _callback -= callback;
        }

        public void Invoke(in T e)
        {
            _callback?.Invoke(e);
        }
    }
}
