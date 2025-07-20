namespace GameFramework
{
    public interface ISystem : IArchitectureModule
    {

    }

    public abstract class AbstractSystem : ISystem, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendCommand, ICanSendQuery, ICanSendEvent, ICanRegisterEvent
    {
        private IArchitecture _architecture = null;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void IArchitectureModule.Init() => OnInit();
        void IArchitectureModule.Destroy() => OnDestroy();
        void IArchitectureModule.SetArchitecture(IArchitecture architecture) => _architecture = architecture;
        IArchitecture IBelongArchitecture.GetArchitecture() => _architecture;
    }
}