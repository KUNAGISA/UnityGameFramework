namespace Framework
{
    public interface ICommandProvider : ICanGetUtility, ICanGetModel, ICanGetSystem, ICanSendCommand, ICanSendQuery, ICanSendEvent
    {

    }

    public interface ICommand
    {
        protected internal void Execute(ICommandProvider provider);
    }

    public interface ICommand<out TResult>
    {
        protected internal TResult Execute(ICommandProvider provider);
    }

    /// <summary>
    /// 无状态处理对象，如果比较频繁，建议使用结构体或者<see cref="UniqueShare{T}"/>
    /// </summary>
    public abstract class AbstractCommand : ICommand
    {
        protected abstract void Execute(ICommandProvider provider);

        void ICommand.Execute(ICommandProvider provider) => Execute(provider);
    }

    /// <summary>
    /// 无状态处理对象，如果比较频繁，建议使用结构体或者<see cref="UniqueShare{T}"/>
    /// </summary>
    public abstract class AbstractCommand<TResult> : ICommand<TResult>
    {
        protected abstract TResult Execute(ICommandProvider provider);

        TResult ICommand<TResult>.Execute(ICommandProvider provider) => Execute(provider);
    }
}
