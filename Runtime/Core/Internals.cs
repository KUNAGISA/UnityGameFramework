using System;

namespace Framework.Internals
{
    public interface IContainer<TBase>
    {
        T Get<T>() where T : class, TBase;
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
        void SendCommand<TCommand>(TCommand command) where TCommand : ICommand;
        TResult SendCommand<TResult, TCommand>() where TCommand : ICommand<TResult>, new();
        TResult SendCommand<TResult, TCommand>(TCommand command) where TCommand : ICommand<TResult>;
    }

    public interface ISendQuery
    {
        TResult SendQuery<TResult, TQuery>() where TQuery : IQuery<TResult>, new();
        TResult SendQuery<TResult, TQuery>(TQuery query) where TQuery : IQuery<TResult>;
    }
}