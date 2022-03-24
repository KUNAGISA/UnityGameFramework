namespace Framework.Internal.Operate
{
    public interface ISendCommand
    {
        /// <summary>
        /// 发送指令
        /// </summary>
        void SendCommand<T>() where T : ICommand, new();

        /// <summary>
        /// 发送指令
        /// </summary>
        void SendCommand<T>(in T command) where T : ICommand;
    }
}
