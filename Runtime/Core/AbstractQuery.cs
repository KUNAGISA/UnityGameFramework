using Framework.Internals;

namespace Framework
{
    public interface IQuery
    {
        public interface IContext : IContainer<ISystem>, IContainer<IModel>, IContainer<IUtility>, ISendEvent, ISendQuery
        {

        }
    }

    public interface IQuery<out TResult> : IQuery
    {
        protected internal TResult Do(IContext context);
    }

    public abstract class AbstractQuery<TResult> : IQuery<TResult>
    {
        TResult IQuery<TResult>.Do(IQuery.IContext context) => Do(context);

        protected abstract TResult Do(IQuery.IContext context);
    }
}