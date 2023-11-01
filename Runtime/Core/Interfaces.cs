using System;

namespace Framework
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

    public interface IEventManager : ISendEvent
    {
        IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent);
        void UnRegisterEvent<TEvent>(Action<TEvent> onEvent);
    }

    public interface ISendCommand
    {
        void SendCommand<TCommand>() where TCommand : ICommand, new();
        void SendCommand<TCommand>(in TCommand command) where TCommand : ICommand;

        TResult SendCommand<TResult, TCommand>() where TCommand : ICommand<TResult>, new();
        TResult SendCommand<TResult, TCommand>(in TCommand command) where TCommand : ICommand<TResult>;
    }

    public interface ISendQuery
    {
        TResult SendQuery<TResult, TQuery>() where TQuery : IQuery<TResult>, new();
        TResult SendQuery<TResult, TQuery>(in TQuery query) where TQuery : IQuery<TResult>;
    }

    public interface IUtilityContext : ISendEvent
    {

    }

    public interface IModelContext : IGetUtility, ISendEvent
    {

    }

    public interface ISystemContext : IGetSystem, IGetModel, IGetUtility, ISendEvent, ISendCommand, ISendQuery, IEventManager
    {

    }

    public interface ICommandContext : IGetSystem, IGetModel, IGetUtility, ISendEvent, ISendCommand, ISendQuery
    {

    }

    public interface IQueryContext : IGetSystem, IGetModel, IGetUtility, ISendQuery
    {

    }

    public interface IArchitecture : ISystemContext, IModelContext, IUtilityContext, ICommandContext, IQueryContext
    {
        void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem;
        void RegisterModel<TModel>(TModel model) where TModel : class, IModel;
        void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility;

        void UnRegisterSystem<TSystem>() where TSystem : class, ISystem;
        void UnRegisterModel<TModel>() where TModel : class, IModel;
        void UnRegisterUtility<TUtility>() where TUtility : class, IUtility;
    }

    public interface IUtility
    {
        void Init();
        void Destroy();
        void SetContext(IUtilityContext context);
    }

    public interface IModel
    {
        void Init();
        void Destroy();
        void SetContext(IModelContext context);
    }

    public interface ISystem
    {
        void Init();
        void Destroy();
        void SetContext(ISystemContext context);
    }

    public interface ICommand
    {
        void Execute(ICommandContext context);
    }

    public interface ICommand<TResult> : ICommand
    {
        new TResult Execute(ICommandContext context);
        void ICommand.Execute(ICommandContext context) => Execute(context);
    }

    public interface IQuery<TResult>
    {
        TResult Do(IQueryContext context);
    }
}