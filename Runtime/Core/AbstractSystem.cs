namespace Framework
{
    public interface ISystem : ISetArchitecture, IInitializable
    {

    }

    public abstract class AbstractSystem : ISystem, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendCommand, ICanSendQuery, ICanSendEvent, ICanRegisterEvent
    {
        private IArchitecture m_architecture = null;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void IInitializable.Init() => OnInit();
        void IInitializable.Destroy() => OnDestroy();
        void ISetArchitecture.SetArchitecture(IArchitecture architecture) => m_architecture = architecture;
        IArchitecture IBelongArchitecture.GetArchitecture() => m_architecture;
    }
}