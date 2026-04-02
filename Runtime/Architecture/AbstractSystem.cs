namespace Aoiro
{
    public interface ISystem : IArchitectureModule, ICanGetSystem, ICanGetModel, ICanGetService, ICanSendCommand, ICanSendQuery, ICanSendEvent, ICanRegisterEvent
    {

    }

    public abstract partial class AbstractSystem : ISystem
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