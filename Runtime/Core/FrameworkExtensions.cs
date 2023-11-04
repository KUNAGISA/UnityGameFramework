namespace Framework
{
    public static class FrameworkExtensions
    {
        public static void SendCommand<TResult, TCommand>(this ISendCommand self, out TResult result, TCommand command) where TCommand : ICommand<TResult>
        {
            result = self.SendCommand<TResult, TCommand>(command);
        }

        public static void SendCommand<TResult, TCommand>(this ISendCommand self, out TResult result) where TCommand : ICommand<TResult>, new()
        {
            result = self.SendCommand<TResult, TCommand>();
        }

        public static void SendQuery<TResult, TQuery>(this ISendQuery self, out TResult result, TQuery query) where TQuery : IQuery<TResult>
        {
            result = self.SendQuery<TResult, TQuery>(query);
        }

        public static void SendQuery<TResult, TQuery>(this ISendQuery self, out TResult result) where TQuery: IQuery<TResult>, new()
        {
            result = self.SendQuery<TResult, TQuery>();
        }
    }
}