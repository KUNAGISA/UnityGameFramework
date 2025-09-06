using System;
using System.Collections.Generic;

namespace GameFramework
{
    public sealed class TypedEventCenter
    {
        private readonly Dictionary<Type, IEventChannel> _channels = new Dictionary<Type, IEventChannel>();

        public void Send<T>(in T e)
        {
            if (_channels.TryGetValue(typeof(EventChannel<T>), out var channel))
            {
                ((EventChannel<T>)channel).Invoke(e);
            }
        }

        public ICancelToken Register<T>(Action<T> callback)
        {
            if (!_channels.TryGetValue(typeof(T), out var channel))
            {
                channel = new EventChannel<T>();
                _channels.Add(typeof(T), channel);
            }
            return ((EventChannel<T>)channel).Register(callback);
        }

        public void Cancel<T>(Action<T> callback)
        {
            if (_channels.TryGetValue(typeof(EventChannel<T>), out var channel))
            {
                ((EventChannel<T>)channel).Cancel(callback);
            }
        }

        public void Clear()
        {
            _channels.Clear();
        }
    }
}