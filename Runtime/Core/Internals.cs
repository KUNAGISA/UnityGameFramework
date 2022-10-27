namespace Framework.Internals
{
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
        void SendEvent<TEvent>() where TEvent : new();

        void SendEvent<TEvent>(TEvent @event);
    }

    public interface ISendCommand
    {
        void SendCommand<TCommand>() where TCommand : ICommand, new();

        void SendCommand<TCommand>(TCommand command) where TCommand : ICommand;
    }

    public interface ISendQuery
    {
        TResult SendQuery<TResult, TQuery>() where TQuery : IQuery<TResult>, new();

        TResult SendQuery<TResult, TQuery>(TQuery query) where TQuery : IQuery<TResult>;
    }
}