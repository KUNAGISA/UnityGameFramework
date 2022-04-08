using Framework.Internal.Operate;

namespace Framework
{
    /// <summary>
    /// 系统逻辑模块基类
    /// </summary>
    public abstract class AbstractSystem : ISystem, IDestory
    {
        private IArchitecture m_Architecture;

        void ISystem.InitSystem() => OnInitSystem();

        void IDestory.Destroy() => OnDestorySystem();

        IArchitecture IBelongArchiecture.GetArchitecture() => m_Architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_Architecture = architecture;

        abstract protected void OnInitSystem();

        abstract protected void OnDestorySystem();
    }
}