namespace Framework
{
    public interface IQueryContext : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendQuery
    {

    }

    public interface IQuery<TResult> : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendQuery
    {
        protected internal TResult Do(IQueryContext context);
    }
}
