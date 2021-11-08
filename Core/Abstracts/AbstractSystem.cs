namespace Framework
{
    /// <summary>
    /// 系统逻辑模块基类
    /// </summary>
    public abstract class AbstractSystem : ISystem
    {
        void ISystem.InitSystem()
        {
            OnInitSystem();
        }
        abstract protected void OnInitSystem();

        private IArchitecture m_Architecture;

        IArchitecture IBelongArchiecture.GetArchitecture()
        {
            return m_Architecture;
        }

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture)
        {
            m_Architecture = architecture;
        }
    }
}