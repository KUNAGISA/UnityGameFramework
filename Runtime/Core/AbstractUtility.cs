namespace Framework
{
    public abstract class AbstractUtility : IUtility
    {
        private IUtilityContext m_context = null;
        protected IUtilityContext Context => m_context;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void IUtility.Init() => OnInit();
        void IUtility.Destroy() => OnDestroy();
        void IUtility.SetContext(IUtilityContext context) => m_context = context;
    }
}