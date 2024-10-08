namespace Framework
{
    public interface IUtility : ISetArchitecture, IInitializable
    {

    }

    public abstract class AbstractUtility : IUtility, ICanSendEvent
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