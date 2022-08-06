namespace Framework
{
    public interface ICanSendCommand : IBelongArchiecture
    {
    }

    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : struct, ICommand
        {
            self.GetArchitecture().SendCommand<T>();
        }

        public static void SendCommand<T>(this ICanSendCommand self, in T command) where T : struct, ICommand
        {
            self.GetArchitecture().SendCommand(command);
        }
    }
}
