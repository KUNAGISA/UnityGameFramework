namespace Framework
{
    public interface IModel : IArchitectureModule
    {

    }

    public abstract class AbstractModel : IModel, ICanGetUtility, ICanSendEvent
    {
        private IArchitecture m_architecutre = null;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void IArchitectureModule.Init() => OnInit();
        void IArchitectureModule.Destroy() => OnDestroy();
        void IArchitectureModule.SetArchitecture(IArchitecture architecutre) => m_architecutre = architecutre;
        IArchitecture IBelongArchitecture.GetArchitecture() => m_architecutre;
    }
}