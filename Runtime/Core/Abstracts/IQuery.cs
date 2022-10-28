namespace Framework
{
    public interface IQueryContext : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendQuery
    {

    }

    public interface IQuery<TResult>
    {
        protected internal TResult Do(IQueryContext context);
    }
}
