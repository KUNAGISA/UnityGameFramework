using Framework.Internals;

namespace Framework
{
    public interface IUtility
    {
        public interface IContext : ISendEvent
        {

        }

        protected internal void Init();
        protected internal void Destroy();
        protected internal void SetContext(IContext context);
    }

    public abstract class AbstractUtility : IUtility
    {
        protected IUtility.IContext Context { get; private set; }

        void IUtility.Init() => OnInit();

        void IUtility.Destroy() => OnDestroy();

        void IUtility.SetContext(IUtility.IContext context) => Context = context;

        protected virtual void OnInit() { }

        protected virtual void OnDestroy() { }
    }
}