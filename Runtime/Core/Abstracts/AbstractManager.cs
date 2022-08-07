using Framework.Internals;

namespace Framework
{
    public interface IManager : IInit, IDestory, ICanSetArchiecture, ICanGetModel, ICanGetSystem, ICanGetManager, ICanGetUtility, ICanRegisterEvent, ICanSendCommand, ICanSendQuery, ICanSendEvent
    {

    }

    public abstract class AbstractManager : IManager
    {
        private IArchitecture m_architecure = null;

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecure;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecure = architecture;

        void IInit.Init() => OnInit();

        void IDestory.Destroy() => OnDestroy();

        protected abstract void OnInit();

        protected abstract void OnDestroy();
    }
}
