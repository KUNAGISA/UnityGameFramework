namespace GameFramework
{
    public interface IService : IArchitectureModule
    {

    }

    public abstract partial class AbstractService : IService, ICanSendEvent
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