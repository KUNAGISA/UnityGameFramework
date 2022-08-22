namespace Framework
{
    public class TypeEventSystem
    {
        private readonly EasyEvents m_events = new EasyEvents();

        public void Send<T>() where T : struct
        {
            m_events.GetEvent<EasyValueEvent<T>>()?.Trigger(new T());
        }

        public void Send<T>(in T @event) where T : struct
        {
            m_events.GetEvent<EasyValueEvent<T>>()?.Trigger(in @event);
        }

        public IUnRegister Register<T>(ValueAction<T> onEvent) where T : struct
        {
            var @event = m_events.GetOrAddEvent<EasyValueEvent<T>>();
            return @event.Register(onEvent);
        }

        public void UnRegister<T>(ValueAction<T> onEvent) where T : struct
        {
            m_events.GetEvent<EasyValueEvent<T>>()?.UnRegister(onEvent);
        }
    }
}