namespace GameFramework
{
    public interface IModel : IArchitectureModule
    {

    }

    public abstract class AbstractModel : IModel, ICanGetUtility, ICanSendEvent
    {
        private IArchitecture _architecture = null;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void IArchitectureModule.Init() => OnInit();
        void IArchitectureModule.Destroy() => OnDestroy();
        void IArchitectureModule.SetArchitecture(IArchitecture architecutre) => _architecture = architecutre;
        IArchitecture IBelongArchitecture.GetArchitecture() => _architecture;
    }
}