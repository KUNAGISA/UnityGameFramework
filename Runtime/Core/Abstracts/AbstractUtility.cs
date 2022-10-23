namespace Framework
{
    public interface IUtility : IBelongArchiecture, ICanSetArchiecture, ICanSendEvent
    {
        protected internal void Destroy();
    }

    public abstract class AbstractUtility : IUtility
    {
        private IArchitecture m_architecture;

        void IUtility.Destroy() => OnDestroy();

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecture = architecture;

        protected virtual void OnDestroy() { }
    }
}