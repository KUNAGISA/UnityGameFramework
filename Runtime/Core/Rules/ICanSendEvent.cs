
namespace Framework
{
    public interface ICanSendEvent : IBelongArchiecture
    {
    }

    public static class CanSendEventExection
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        public static void SendEvent<T>(this ICanSendEvent self, in T e)
        {
            self.GetArchitecture().SendEvent(e);
        }
    }
}
