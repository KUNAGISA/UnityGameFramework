using Framework.Internal.Operate;

namespace Framework
{
    /// <summary>
    /// 工具基类
    /// </summary>
    public abstract class AbstractUtility : IUtility
    {
        private IArchitecture m_architecture;

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecture = architecture;

        void IDestory.Destroy() => OnDestroy();

        protected virtual void OnDestroy() { }
    }
}