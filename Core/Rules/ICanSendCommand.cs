
namespace Framework
{
    public interface ICanSendCommand : IBelongArchiecture
    {
    }

    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        public static void SendCommand<T>(this ICanSendCommand self, T e) where T : ICommand
        {
            self.GetArchitecture().SendCommand(e);
        }
    }
}
