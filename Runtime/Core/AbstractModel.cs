namespace Framework
{
    public interface IModel : ISetArchitecture, ICanInit, ICanGetUtility, ICanSendEvent
    {

    }

    public abstract class AbstractModel : IModel
    {
        private IArchitecture m_architecutre = null;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void ICanInit.Init() => OnInit();
        void ICanInit.Destroy() => OnDestroy();
        void ISetArchitecture.SetArchitecture(IArchitecture architecutre) => m_architecutre = architecutre;
        IArchitecture IBelongArchitecture.GetArchitecture() => m_architecutre;
    }
}