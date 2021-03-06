namespace Framework.Internal.Operate
{
    public interface ISendEvent
    {
        /// <summary>
        /// 发送事件
        /// </summary>
        void SendEvent<T>() where T : new();

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="event">事件</param>
        void SendEvent<T>(in T @event);
    }
}
