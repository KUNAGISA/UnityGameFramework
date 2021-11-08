namespace Framework
{
    /// <summary>
    /// 查询数据基类
    /// 一般用于跨数据模块查询
    /// </summary>
    /// <typeparam name="TResult">返回的数据类型</typeparam>
    public abstract class AbstractQuery<TResult> : IQuery<TResult>
    {
        private IArchitecture m_Archiecture;

        TResult IQuery<TResult>.Do()
        {
            return OnDo();
        }

        protected abstract TResult OnDo();

        IArchitecture IBelongArchiecture.GetArchitecture()
        {
            return m_Archiecture;
        }

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture)
        {
            m_Archiecture = architecture;
        }
    }
}
