﻿namespace Framework
{
    public interface IQueryContext : ICanGetUtility, ICanGetModel, ICanGetSystem, ICanSendQuery
    {

    }

    public interface IQuery<out TResult>
    {
        TResult Do(IQueryContext context);
    }

    /// <summary>
    /// 无状态查询对象，如果比较频繁，建议使用结构体或者<see cref="UniqueShare{T}"/>
    /// </summary>
    public abstract class AbstractQuery<TResult> : IQuery<TResult>
    {
        protected abstract TResult Do(IQueryContext context);

        TResult IQuery<TResult>.Do(IQueryContext context) => Do(context);
    }
}
