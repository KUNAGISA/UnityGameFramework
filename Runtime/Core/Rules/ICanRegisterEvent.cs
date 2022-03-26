namespace Framework
{
    public interface ICanRegisterEvent : IBelongArchiecture
    {
    }

    public static class CanRegisterEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, IEventSystem.OnEventHandler<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent(onEvent);
        }

        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, IEventSystem.OnEventHandler<T> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent(onEvent);
        }
    }

}
