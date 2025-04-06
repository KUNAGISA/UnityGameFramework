namespace Framework
{
    public interface ISystem : IArchitectureModule
    {

    }

    public abstract class AbstractSystem : ISystem, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendCommand, ICanSendQuery, ICanSendEvent, ICanRegisterEvent
    {
        private IArchitecture m_architecture = null;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void IArchitectureModule.Init() => OnInit();
        void IArchitectureModule.Destroy() => OnDestroy();
        void IArchitectureModule.SetArchitecture(IArchitecture architecture) => m_architecture = architecture;
        IArchitecture IBelongArchitecture.GetArchitecture() => m_architecture;
    }
}