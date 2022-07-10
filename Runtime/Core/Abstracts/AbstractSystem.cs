using Framework.Internal.Operate;

namespace Framework
{
    /// <summary>
    /// 系统逻辑模块基类
    /// </summary>
    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture m_architecture;

        void ISystem.InitSystem() => OnInitSystem();

        void IDestory.Destroy() => OnDestorySystem();

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecture = architecture;

        abstract protected void OnInitSystem();

        abstract protected void OnDestorySystem();
    }
}