using System;

namespace Framework
{
    public interface IBelongArchiecture
    {
        protected internal IArchitecture GetArchitecture();
    }

    public interface ICanSetArchiecture
    {
        void SetArchiecture(IArchitecture architecture);
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

        public static TResult SendQuery<TResult, TQuery>(this ICanSendQuery self, TQuery query) where TQuery : IQuery<TResult>, new()
        {
            return self.GetArchitecture().SendQuery<TResult, TQuery>();
        }

        public static bool SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<bool>, new()
        {
            return self.GetArchitecture().SendQuery<bool, TQuery>();
        }

        public static bool SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<bool>
        {
            return self.GetArchitecture().SendQuery<bool, TQuery>(query);
        }

        public static byte SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<byte>, new()
        {
            return self.GetArchitecture().SendQuery<byte, TQuery>();
        }

        public static byte SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<byte>
        {
            return self.GetArchitecture().SendQuery<byte, TQuery>(query);
        }

        public static char SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<char>, new()
        {
            return self.GetArchitecture().SendQuery<char, TQuery>();
        }

        public static char SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<char>
        {
            return self.GetArchitecture().SendQuery<char, TQuery>(query);
        }

        public static decimal SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<decimal>, new()
        {
            return self.GetArchitecture().SendQuery<decimal, TQuery>();
        }

        public static decimal SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<decimal>
        {
            return self.GetArchitecture().SendQuery<decimal, TQuery>(query);
        }

        public static double SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<double>, new()
        {
            return self.GetArchitecture().SendQuery<double, TQuery>();
        }

        public static double SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<double>
        {
            return self.GetArchitecture().SendQuery<double, TQuery>(query);
        }

        public static float SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<float>, new()
        {
            return self.GetArchitecture().SendQuery<float, TQuery>();
        }

        public static float SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<float>
        {
            return self.GetArchitecture().SendQuery<float, TQuery>(query);
        }

        public static int SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<int>, new()
        {
            return self.GetArchitecture().SendQuery<int, TQuery>();
        }

        public static int SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<int>
        {
            return self.GetArchitecture().SendQuery<int, TQuery>(query);
        }

        public static long SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<long>, new()
        {
            return self.GetArchitecture().SendQuery<long, TQuery>();
        }

        public static long SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<long>
        {
            return self.GetArchitecture().SendQuery<long, TQuery>(query);
        }

        public static sbyte SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<sbyte>, new()
        {
            return self.GetArchitecture().SendQuery<sbyte, TQuery>();
        }

        public static sbyte SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<sbyte>
        {
            return self.GetArchitecture().SendQuery<sbyte, TQuery>(query);
        }

        public static short SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<short>, new()
        {
            return self.GetArchitecture().SendQuery<short, TQuery>();
        }

        public static short SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<short>
        {
            return self.GetArchitecture().SendQuery<short, TQuery>(query);
        }

        public static uint SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<uint>, new()
        {
            return self.GetArchitecture().SendQuery<uint, TQuery>();
        }

        public static uint SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<uint>
        {
            return self.GetArchitecture().SendQuery<uint, TQuery>(query);
        }

        public static ulong SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<ulong>, new()
        {
            return self.GetArchitecture().SendQuery<ulong, TQuery>();
        }

        public static ulong SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<ulong>
        {
            return self.GetArchitecture().SendQuery<ulong, TQuery>(query);
        }

        public static ushort SendQuery<TQuery>(this ICanSendQuery self) where TQuery : IQuery<ushort>, new()
        {
            return self.GetArchitecture().SendQuery<ushort, TQuery>();
        }

        public static ushort SendQuery<TQuery>(this ICanSendQuery self, TQuery quert) where TQuery : IQuery<ushort>
        {
            return self.GetArchitecture().SendQuery<ushort, TQuery>(query);
        }
    }
}
