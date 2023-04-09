using Framework.Internals;

namespace Framework
{
    public interface ICommand
    {
        public interface IContext : IContainer<ISystem>, IContainer<IModel>, IContainer<IUtility>, ISendEvent, ISendCommand, ISendQuery
        {

        }

        internal protected void Execute(IContext context);
    }

    public interface ICommand<out TResult> : ICommand
    {
        internal protected new TResult Execute(IContext context);
        void ICommand.Execute(IContext context) => Execute(context);
    }

    public abstract class AbstractCommand : ICommand
    {
        void ICommand.Execute(ICommand.IContext context) => Execute(context);

        protected abstract void Execute(ICommand.IContext context);
    }

    public abstract class AbstractCommand<TResult> : ICommand<TResult>
    {
        TResult ICommand<TResult>.Execute(ICommand.IContext context) => Execute(context);

        protected abstract TResult Execute(ICommand.IContext context);
    }
}