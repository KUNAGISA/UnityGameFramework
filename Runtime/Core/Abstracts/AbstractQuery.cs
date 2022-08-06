namespace Framework
{
    /// <summary>
    /// 查询基类
    /// </summary>
    /// <typeparam name="TResult">查询结果类型</typeparam>
    public abstract class AbstractQuery<TResult> : IQuery<TResult>
    {
        public abstract TResult Do(IQueryArchitecture architecture);
    }
}
