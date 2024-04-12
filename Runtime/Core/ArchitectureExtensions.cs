using System;

namespace Framework
{
    public interface IBelongArchitecture
    {
        protected internal IArchitecture GetArchitecture();
    }

    public interface ISetArchitecture
    {
        protected internal void SetArchitecture(IArchitecture architecture);
    }

    public interface ICanInit
    {
        protected internal void Init();
        protected internal void Destroy();
    }

    public interface ICanGetUtility : IBelongArchitecture
    {

    }

    public interface ICanGetModel : IBelongArchitecture
    {

    }

    public interface ICanGetSystem : IBelongArchitecture
    {

    }

    public interface ICanSendCommand : IBelongArchitecture
    {

    }

    public interface ICanSendQuery : IBelongArchitecture
    {

    }

    public interface ICanRegisterEvent : IBelongArchitecture
    {

    }

    public interface ICanSendEvent : IBelongArchitecture
    {

    }

    public static class ArchitectureExtensions
    {
        public static TUtility GetUtility<TUtility>(this ICanGetUtility context) where TUtility : class, IUtility
        {
            return context.GetArchitecture().GetUtility<TUtility>();
        }

        public static TModel GetModel<TModel>(this ICanGetModel context) where TModel : class, IModel
        {
            return context.GetArchitecture().GetModel<TModel>();
        }

        public static TSystem GetSystem<TSystem>(this ICanGetSystem context) where TSystem : class, ISystem
        {
            return context.GetArchitecture().GetSystem<TSystem>();
        }

        /// <summary>
        /// Call a command.
        /// </summary>
        public static void SendCommand<TCommand>(this ICanSendCommand context, TCommand command) where TCommand : ICommand
        {
            context.GetArchitecture().SendCommand(command);
        }

        /// <summary>
        /// Call a command and return result.Use <see cref="SendCommand{TResult, TCommand}(ICanSendCommand, TCommand)"/> if command is value type, or it will be boxed.
        /// </summary>
        public static TResult SendCommand<TResult>(this ICanSendCommand context, ICommand<TResult> command)
        {
            return context.GetArchitecture().SendCommand(command);
        }

        /// <summary>
        /// Call a command and return result.
        /// </summary>
        public static TResult SendCommand<TResult, TCommand>(this ICanSendCommand context, TCommand command) where TCommand : struct, ICommand<TResult>
        {
            return context.GetArchitecture().SendCommand<TResult, TCommand>(command);
        }

        /// <summary>
        /// Call a query and return result.Use <see cref="SendQuery{TResult, TQuery}(ICanSendQuery, TQuery)"/> if query is value type, or it will be boxed.
        /// </summary>
        public static TResult SendQuery<TResult>(this ICanSendQuery context, IQuery<TResult> command)
        {
            return context.GetArchitecture().SendQuery(command);
        }

        /// <summary>
        /// Call a query and return result.
        /// </summary>
        public static TResult SendQuery<TResult, TQuery>(this ICanSendQuery context, TQuery command) where TQuery : struct, IQuery<TResult>
        {
            return context.GetArchitecture().SendQuery<TResult, TQuery>(command);
        }

        public static IUnRegister RegisterEvent<TEvent>(this ICanRegisterEvent context, Action<TEvent> onEvent)
        {
            return context.GetArchitecture().RegisterEvent(onEvent);
        }

        public static void UnRegisterEvent<TEvent>(this ICanRegisterEvent context, Action<TEvent> onEvent)
        {
            context.GetArchitecture().UnRegisterEvent(onEvent);
        }

        public static void SendEvent<TEvent>(this ICanSendEvent context) where TEvent : new()
        {
            context.GetArchitecture().SendEvent<TEvent>();
        }

        public static void SendEvent<TEvent>(this ICanSendEvent context, TEvent @event)
        {
            context.GetArchitecture().SendEvent(@event);
        }
    }
}