namespace Framework
{
    public interface IUtility : ISetArchitecture, ICanInit, ICanSendEvent
    {

    }

    public abstract class AbstractUtility : IUtility
    {
        private IArchitecture m_architecture = null;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void ICanInit.Init() => OnInit();
        void ICanInit.Destroy() => OnDestroy();
        void ISetArchitecture.SetArchitecture(IArchitecture architecture) => m_architecture = architecture;
        IArchitecture IBelongArchitecture.GetArchitecture() => m_architecture;
    }
}