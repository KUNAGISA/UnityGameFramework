namespace Framework
{
    public abstract class AbstractSystem : ISystem
    {
        private ISystemContext m_context = null;
        protected ISystemContext Context => m_context;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void ISystem.Init() => OnInit();
        void ISystem.Destroy() => OnDestroy();
        void ISystem.SetContext(ISystemContext context) => m_context = context;
    }
}