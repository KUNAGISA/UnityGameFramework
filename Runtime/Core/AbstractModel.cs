namespace Framework
{
    public abstract class AbstractModel : IModel
    {
        private IModelContext m_context = null;
        protected IModelContext Context => m_context;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void IModel.Init() => OnInit();
        void IModel.Destroy() => OnDestroy();
        void IModel.SetContext(IModelContext context) => m_context = context;
    }
}