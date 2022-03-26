namespace Framework
{
    /// <summary>
    /// 工具基类
    /// </summary>
    public abstract class AbstractUtility : IUtility, ICanGetUtility
    {
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