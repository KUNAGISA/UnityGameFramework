using Framework.Internals;

namespace Framework
{
    public interface IModel
    {
        public interface IContext : IContainer<IUtility>, ISendEvent
        {

        }

        protected internal void Init();
        protected internal void Destroy();
        protected internal void SetContext(IContext context);
    }

    public abstract class AbstractModel : IModel
    {
        protected IModel.IContext Context { get; private set; }

        void IModel.Init() => OnInit();

        void IModel.Destroy() => OnDestroy();

        void IModel.SetContext(IModel.IContext context) => Context = context;

        protected virtual void OnInit() { }

        protected virtual void OnDestroy() { }
    }
}