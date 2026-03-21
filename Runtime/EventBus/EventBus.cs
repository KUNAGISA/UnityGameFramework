using System;
using System.Collections.Generic;

namespace GameFramework
{
    public sealed class EventBus
    {
        private readonly Dictionary<Type, ISignal> _signals = new();

        public void Send<T>(in T e)
        {
            if (_signals.TryGetValue(typeof(Signal<T>), out var signal))
            {
                ((Signal<T>)signal).Emit(e);
            }
        }

        public SignalToken Register<T>(Action<T> callback)
        {
            var key = typeof(Signal<T>);
            if (!_signals.TryGetValue(key, out var signal))
            {
                signal = new Signal<T>();
                _signals.Add(key, signal);
            }
            return ((Signal<T>)signal).Register(callback);
        }
        
        public void Clear()
        {
            foreach (var (_, signal) in _signals)
            {
                signal.Cleanup();
            }
            _signals.Clear();
        }
    }
}