using Framework.Internals;

namespace Framework
{
    public interface ISystem : IInit, IDestory, IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanGetModel, ICanGetSystem, ICanRegisterEvent, ICanSendEvent, ICanSendQuery
    {

    }

    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture m_architecture;

        void IInit.Init() => OnInit();

        void IDestory.Destroy() => OnDestroy();

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecture = architecture;

        abstract protected void OnInit();

        abstract protected void OnDestroy();
    }
}