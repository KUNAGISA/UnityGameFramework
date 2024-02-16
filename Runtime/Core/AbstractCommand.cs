namespace Framework
{
    public interface ICommandContext : ICanGetUtility, ICanGetModel, ICanGetSystem, ICanSendCommand, ICanSendQuery, ICanSendEvent
    {

    }

    public interface ICommand
    {
        void Execute(ICommandContext context);
    }

    public interface ICommand<out TResult>
    {
        TResult Execute(ICommandContext context);
    }

    /// <summary>
    /// 无状态处理对象，如果比较频繁，建议使用结构体或者<see cref="UniqueShare{T}"/>
    /// </summary>
    public abstract class AbstractCommand : ICommand
    {
        protected abstract void Execute(ICommandContext context);

        void ICommand.Execute(ICommandContext context) => Execute(context);
    }

    /// <summary>
    /// 无状态处理对象，如果比较频繁，建议使用结构体或者<see cref="UniqueShare{T}"/>
    /// </summary>
    public abstract class AbstractCommand<TResult> : ICommand<TResult>
    {
        protected abstract TResult Execute(ICommandContext context);

        TResult ICommand<TResult>.Execute(ICommandContext context) => Execute(context);
    }
}
