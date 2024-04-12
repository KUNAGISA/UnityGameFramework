namespace Framework
{
    public interface IQueryProvider : ICanGetUtility, ICanGetModel, ICanGetSystem, ICanSendQuery
    {

    }

    public interface IQuery<out TResult>
    {
        protected internal TResult Do(IQueryProvider provider);
    }

    /// <summary>
    /// 无状态查询对象，如果比较频繁，建议使用结构体或者<see cref="UniqueShare{T}"/>
    /// </summary>
    public abstract class AbstractQuery<TResult> : IQuery<TResult>
    {
        protected abstract TResult Do(IQueryProvider provider);

        TResult IQuery<TResult>.Do(IQueryProvider provider) => Do(provider);
    }
}
