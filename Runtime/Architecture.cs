using System;
using System.Collections.Generic;

namespace GameFramework
{
    public partial interface IArchitecture
    {
        bool Contains<T>() where T : class;
        void Register<T>(T instance) where T : class, IArchitectureModule;
        void UnRegister<T>() where T : class, IArchitectureModule;
        T Get<T>() where T : class, IArchitectureModule;
        IEnumerable<T> Select<T>() where T : class;

        void SendCommand<TCommand>(TCommand command) where TCommand : ICommand;

        TResult SendCommand<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;

        TResult SendQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;

        ICancelToken RegisterEvent<TEvent>(Action<TEvent> onEvent);
        void CancelEvent<TEvent>(Action<TEvent> onEvent);

        void SendEvent<TEvent>(in TEvent e);
    }

    public abstract partial class Architecture<TArchitecture> : IArchitecture, ICommandContext, IQueryContext where TArchitecture : Architecture<TArchitecture>, new()
    {
        public static event Action<TArchitecture> OnRegisterPatch;

        private static TArchitecture s_architecture = null;
        public static IArchitecture Instance
        {
            get
            {
                if (s_architecture == null)
                {
                    MakeSureArchitecture();
                }
                return s_architecture;
            }
        }

        public static bool Valid => s_architecture != null;

        public static void MakeSureArchitecture()
        {
            if (s_architecture != null)
            {
                return;
            }

            s_architecture = new TArchitecture();
            s_architecture.OnInit();

            OnRegisterPatch?.Invoke(s_architecture);

            foreach(var utility in s_architecture._iocContainer.Select<IUtility>())
            {
                utility.Init();
            }
            foreach (var model in s_architecture._iocContainer.Select<IModel>())
            {
                model.Init();
            }
            foreach (var system in s_architecture._iocContainer.Select<ISystem>())
            {
                system.Init();
            }

            s_architecture._initialize = true;
        }

        public static void DestroyInstance()
        {
            if (s_architecture == null)
            {
                return;
            }

            foreach (var system in s_architecture._iocContainer.Select<ISystem>())
            {
                system.Destroy();
                system.SetArchitecture(null);
            }
            foreach (var model in s_architecture._iocContainer.Select<IModel>())
            {
                model.Destroy();
                model.SetArchitecture(null);
            }
            foreach (var utility in s_architecture._iocContainer.Select<IUtility>())
            {
                utility.Destroy();
                utility.SetArchitecture(null);
            }

            s_architecture._iocContainer.Clear();
            s_architecture._events.Clear();

            s_architecture.OnDestroy();
            s_architecture = null;
        }

        private bool _initialize = false;
        private readonly TypedEvent _events = new TypedEvent();
        private readonly IOCContainer _iocContainer = new IOCContainer();

        protected abstract void OnInit();
        protected abstract void OnDestroy();

        IArchitecture IBelongArchitecture.GetArchitecture() => this;

        public bool Contains<T>() where T : class
        {
            return _iocContainer.Contains<T>();
        }

        public void Register<T>(T instance) where T : class, IArchitectureModule
        {
            UnRegister<T>();

            instance.SetArchitecture(this);
            _iocContainer.Register(instance);

            if (_initialize)
            {
                instance.Init();
            }
        }

        public void UnRegister<T>() where T : class, IArchitectureModule
        {
            if (_iocContainer.UnRegister<T>(out var instance))
            {
                instance.Destroy();
                instance.SetArchitecture(null);
            }
        }

        public virtual T Get<T>() where T : class, IArchitectureModule
        {
            return _iocContainer.Get<T>();
        }

        public IEnumerable<T> Select<T>() where T : class
        {
            return _iocContainer.Select<T>();
        }

        public virtual void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.Execute(this);
        }

        public virtual TResult SendCommand<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            return command.Execute(this);
        }

        public virtual TResult SendQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            return query.Do(this);
        }

        public virtual ICancelToken RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return _events.Register(onEvent);
        }

        public virtual void CancelEvent<TEvent>(Action<TEvent> onEvent)
        {
            _events.Cancel(onEvent);
        }

        public virtual void SendEvent<TEvent>(in TEvent e)
        {
            _events.Send(e);
        }
    }
}