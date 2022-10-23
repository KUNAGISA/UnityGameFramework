using Framework.Internals;

namespace Framework
{
    public interface ISystem : IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanGetModel, ICanGetSystem, ICanRegisterEvent, ICanSendCommand, ICanSendEvent, ICanSendQuery
    {
        protected internal void Init();

        protected internal void Destroy();
    }

    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture m_architecture;

        void ISystem.Init() => OnInit();

        void ISystem.Destroy() => OnDestroy();

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecture = architecture;

        protected virtual void OnInit() { }

        protected virtual void OnDestroy() { }
    }
}