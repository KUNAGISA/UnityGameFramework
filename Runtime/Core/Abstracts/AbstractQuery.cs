namespace Framework
{
    public interface IQuery<TResult> : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendQuery
    {
        internal IArchitecture architecture { get; set; }

        IArchitecture IBelongArchiecture.GetArchitecture() => architecture;

        protected internal TResult Do();
    }

    /// <summary>
    /// 提供一个class的Query，也可以直接继承IQuery做个struct的
    /// </summary>
    public abstract class AbstractQuery<TResult> : IQuery<TResult>
    {
        IArchitecture IQuery<TResult>.architecture { get; set; }

        TResult IQuery<TResult>.Do() => Do();

        protected abstract TResult Do();
    }
}
