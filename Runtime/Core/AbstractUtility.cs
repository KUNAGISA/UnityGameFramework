namespace GameFramework
{
    public interface IUtility : IArchitectureModule
    {

    }

    public abstract partial class AbstractUtility : IUtility, ICanSendEvent
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