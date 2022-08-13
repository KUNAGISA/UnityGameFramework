namespace Framework.Internals
{
    public interface IInit
    {
        internal void Init();
    }

    public interface IDestory
    {
        internal void Destroy();
    }

    public interface IGetSystem
    {
        TSystem GetSystem<TSystem>() where TSystem : class, ISystem;
    }

    public interface IGetModel
    {
        TModel GetModel<TModel>() where TModel : class, IModel;
    }

    public interface IGetUtility
    {
        TUtility GetUtility<TUtility>() where TUtility : class, IUtility;
    }

    public interface ISendEvent
    {
        void SendEvent<TEvent>() where TEvent : struct;

        void SendEvent<TEvent>(in TEvent @event) where TEvent : struct;
    }

    public interface ISendCommand
    {
        void SendCommand<TCommand>() where TCommand : struct, ICommand;

        void SendCommand<TCommand>(in TCommand command) where TCommand : struct, ICommand;
    }

    public interface ISendQuery
    {
        void SendQuery<TQuery, TResult>(out TResult result) where TQuery : struct, IQuery<TResult>;

        void SendQuery<TQuery, TResult>(in TQuery query, out TResult result) where TQuery : struct, IQuery<TResult>;
    }
}