using Framework.Internal.Operate;

namespace Framework
{
    /// <summary>
    /// 工具基类
    /// </summary>
    public abstract class AbstractUtility : IUtility, IDestory
    {
        private IArchitecture m_Architecture;

        void IDestory.Destroy() => OnDestoryUtility();

        IArchitecture IBelongArchiecture.GetArchitecture() => m_Architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_Architecture = architecture;

        abstract protected void OnDestoryUtility();
    }
}