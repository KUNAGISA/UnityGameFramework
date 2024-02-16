using System;

namespace Framework
{
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

        public static void SendCommand<TCommand>(this ICanSendCommand context, TCommand command) where TCommand : ICommand
        {
            context.GetArchitecture().SendCommand(command);
        }

        public static TResult SendCommand<TResult>(this ICanSendCommand context, ICommand<TResult> command)
        {
            return context.GetArchitecture().SendCommand(command);
        }

        public static TResult SendCommand<TResult, TCommand>(this ICanSendCommand context, TCommand command) where TCommand : struct, ICommand<TResult>
        {
            return context.GetArchitecture().SendCommand<TResult, TCommand>(command);
        }

        public static void SendCommand<TResult, TCommand>(this ICanSendCommand context, TCommand command, out TResult result) where TCommand : struct, ICommand<TResult>
        {
            result = context.GetArchitecture().SendCommand<TResult, TCommand>(command);
        }

        public static TResult SendQuery<TResult>(this ICanSendQuery context, IQuery<TResult> command)
        {
            return context.GetArchitecture().SendQuery(command);
        }

        public static TResult SendQuery<TResult, TQuery>(this ICanSendQuery context, TQuery command) where TQuery : struct, IQuery<TResult>
        {
            return context.GetArchitecture().SendQuery<TResult, TQuery>(command);
        }

        public static void SendQuery<TResult, TQuery>(this ICanSendQuery context, TQuery command, out TResult result) where TQuery : struct, IQuery<TResult>
        {
            result = context.GetArchitecture().SendQuery<TResult, TQuery>(command);
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