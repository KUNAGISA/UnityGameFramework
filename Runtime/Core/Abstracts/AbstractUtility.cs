using Framework.Internals;

namespace Framework
{
    public interface IUtility : IDestory, IBelongArchiecture, ICanSetArchiecture, ICanSendEvent
    {

    }

    public abstract class AbstractUtility : IUtility
    {
        private IArchitecture m_architecture;

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecture = architecture;

        void IDestory.Destroy() => OnDestroy();

        protected virtual void OnDestroy() { }
    }
}