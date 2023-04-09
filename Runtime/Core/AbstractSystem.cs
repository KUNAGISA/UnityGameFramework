using Framework.Internals;

namespace Framework
{
    public interface ISystem
    {
        public interface IContext : IContainer<ISystem>, IContainer<IModel>, IContainer<IUtility>, IEventManager, ISendCommand, ISendQuery
        {

        }

        protected internal void Init();
        protected internal void Destroy();
        protected internal void SetContext(IContext context);
    }

    public abstract class AbstractSystem : ISystem
    {
        protected ISystem.IContext Context { get; private set; }

        void ISystem.Init() => OnInit();

        void ISystem.Destroy() => OnDestroy();

        void ISystem.SetContext(ISystem.IContext context) => Context = context;

        protected virtual void OnInit() { }

        protected virtual void OnDestroy() { }
    }
}