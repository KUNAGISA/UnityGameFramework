using Framework.Internal.Operate;

namespace Framework
{
    /// <summary>
    /// 工具基类
    /// </summary>
    public abstract class AbstractUtility : IUtility
    {
        private IArchitecture m_Architecture;

        IArchitecture IBelongArchiecture.GetArchitecture() => m_Architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_Architecture = architecture;
    }
}