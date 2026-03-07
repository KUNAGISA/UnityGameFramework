namespace GameFramework
{
    public interface ISystem : IArchitectureModule
    {

    }

    public abstract partial class AbstractSystem : ISystem, ICanGetSystem, ICanGetModel, ICanGetService, ICanSendCommand, ICanSendQuery, ICanSendEvent, ICanRegisterEvent
    {
        private IArchitecture _architecture = null;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        protected virtual void OnArchitectureReady(IArchitecture architecture) { }

        void IArchitectureModule.Init() => OnInit();
        void IArchitectureModule.Destroy() => OnDestroy();
        IArchitecture IBelongArchitecture.GetArchitecture() => _architecture;

        void IArchitectureModule.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
            OnArchitectureReady(architecture);
        }
    }
}