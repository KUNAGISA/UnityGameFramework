using System;

namespace Framework
{
    public interface IBelongArchiecture
    {
        protected internal IArchitecture GetArchitecture();
    }

    public interface ICanSetArchiecture
    {
        protected internal void SetArchiecture(IArchitecture architecture);
    }

    public interface ICanGetModel : IBelongArchiecture { }

    public interface ICanGetSystem : IBelongArchiecture { }

    public interface ICanGetUtility : IBelongArchiecture { }

    public interface ICanRegisterEvent : IBelongArchiecture { }

    public interface ICanSendEvent : IBelongArchiecture { }

    public interface ICanSendCommand : IBelongArchiecture { }

    public interface ICanSendQuery : IBelongArchiecture { }

    public static class FrameworkRulesExection
    {
        public static TSystem GetSystem<TSystem>(this ICanGetSystem self) where TSystem : class, ISystem
        {
            return self.GetArchitecture().GetSystem<TSystem>();
        }

        static public TModel GetModel<TModel>(this ICanGetModel self) where TModel : class, IModel
        {
            return self.GetArchitecture().GetModel<TModel>();
        }

        public static TUtility GetUtility<TUtility>(this ICanGetUtility self) where TUtility : class, IUtility
        {
            return self.GetArchitecture().GetUtility<TUtility>();
        }

        public static IUnRegister RegisterEvent<TEvent>(this ICanRegisterEvent self, Action<TEvent> onEvent)
        {
            return self.GetArchitecture().RegisterEvent(onEvent);
        }

        public static void UnRegisterEvent<TEvent>(this ICanRegisterEvent self, Action<TEvent> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent(onEvent);
        }

        public static void SendEvent<TEvent>(this ICanSendEvent self) where TEvent : struct
        {
            self.GetArchitecture().SendEvent<TEvent>();
        }

        public static void SendEvent<TEvent>(this ICanSendEvent self, TEvent e)
        {
            self.GetArchitecture().SendEvent(e);
        }

        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand(command);
        }

        public static TResult SendQuery<TResult, TQuery>(this ICanSendQuery self, TQuery query) where TQuery : IQuery<TResult>
        {
            return self.GetArchitecture().SendQuery<TResult, TQuery>(query);
        }

        public static TResult SendQuery<TResult, TQuery>(this ICanSendQuery self) where TQuery : IQuery<TResult>, new()
        {
            return self.GetArchitecture().SendQuery<TResult, TQuery>();
        }

        public static void SendQuery<TResult, TQuery>(this ICanSendQuery self, TQuery query, out TResult output) where TQuery : IQuery<TResult>
        {
            output = self.GetArchitecture().SendQuery<TResult, TQuery>(query);
        }

        public static void SendQuery<TResult, TQuery>(this ICanSendQuery self, out TResult output) where TQuery : IQuery<TResult>, new()
        {
            output = self.GetArchitecture().SendQuery<TResult, TQuery>();
        }
    }
}
