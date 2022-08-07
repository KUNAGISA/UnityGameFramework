using Framework.Internals;

namespace Framework
{
    public interface IModel : IInit, IDestory, IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanSendEvent
    {

    }

    public abstract class AbstractModel : IModel
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