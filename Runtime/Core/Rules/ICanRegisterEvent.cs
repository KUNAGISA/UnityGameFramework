namespace Framework
{
    public interface ICanRegisterEvent : IBelongArchiecture
    {
    }

    public static class CanRegisterEventExtension
    {
        public static IUnRegister RegisterEvent<TEvent>(this ICanRegisterEvent self, IEventSystem.OnEventHandler<TEvent> onEvent) where TEvent : struct
        {
            return self.GetArchitecture().RegisterEvent(onEvent);
        }

        public static void UnRegisterEvent<TEvent>(this ICanRegisterEvent self, IEventSystem.OnEventHandler<TEvent> onEvent) where TEvent : struct
        {
            self.GetArchitecture().UnRegisterEvent(onEvent);
        }
    }

}
