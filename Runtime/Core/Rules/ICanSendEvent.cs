
namespace Framework
{
    public interface ICanSendEvent : IBelongArchiecture
    {
    }

    public static class CanSendEventExection
    {
        public static void SendEvent<TEvent>(this ICanSendEvent self) where TEvent : struct
        {
            self.GetArchitecture().SendEvent<TEvent>();
        }

        public static void SendEvent<TEvent>(this ICanSendEvent self, in TEvent e) where TEvent : struct
        {
            self.GetArchitecture().SendEvent(e);
        }
    }
}
