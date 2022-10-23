namespace Framework
{
    public interface IModel : IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanSendEvent
    {
        protected internal void Init();

        protected internal void Destroy();
    }

    public abstract class AbstractModel : IModel
    {
        private IArchitecture m_architecture;

        void IModel.Init() => OnInit();

        void IModel.Destroy() => OnDestroy();

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecture = architecture;

        protected virtual void OnInit() { }

        protected virtual void OnDestroy() { }
    }
}