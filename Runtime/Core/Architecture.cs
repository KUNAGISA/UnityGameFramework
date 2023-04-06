using System;

namespace Framework
{
    internal readonly struct ArchitectureWorkspace : IDisposable
    {
        internal static IArchitecture ExecutingArchitecture = null;

        private readonly IArchitecture m_recordArchitecture;

        internal ArchitectureWorkspace(IArchitecture architecture)
        {
            m_recordArchitecture = ExecutingArchitecture;
            ExecutingArchitecture = architecture;
        }

        void IDisposable.Dispose()
        {
            ExecutingArchitecture = m_recordArchitecture;
        }
    }

    public interface IArchitecture
    {
        void RegisterSystem<T>(T system) where T : class, ISystem;

        void RegisterModel<T>(T model) where T : class, IModel;

        void RegisterUtility<T>(T utility) where T : class, IUtility;

        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        void UnRegisterEvent<T>(Action<T> onEvent);

        TSystem GetSystem<TSystem>() where TSystem : class, ISystem;

        TModel GetModel<TModel>() where TModel : class, IModel;

        TUtility GetUtility<TUtility>() where TUtility : class, IUtility;

        void SendCommand<TCommand>() where TCommand : ICommand, new();

        void SendCommand<TCommand>(TCommand command) where TCommand : ICommand;

        TResult SendCommand<TCommand, TResult>() where TCommand : ICommand<TResult>, new();

        TResult SendCommand<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;

        TResult SendQuery<TQuery, TResult>() where TQuery : IQuery<TResult>, new();

        TResult SendQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;

        void SendEvent<TEvent>() where TEvent : new();

        void SendEvent<TEvent>(TEvent @event);

        void Inject<TClass>(TClass instance) where TClass : class;

        void Inject<TStruct>(ref TStruct instance) where TStruct : struct;
    }

    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, IArchitecture, new()
    {
        private static T m_architecture = null;
        public static IArchitecture Instance
        {
            get
            {
                if (m_architecture == null)
                {
                    MakeSureArchitecture();
                }
                return m_architecture;
            }
        }

        public static event Action<T> OnRegisterPatch;

        public static bool IsValid => m_architecture != null;

        public static void MakeSureArchitecture()
        {
            if (m_architecture != null)
            {
                return;
            }

            m_architecture = new T();
            m_architecture.OnInit();

            OnRegisterPatch?.Invoke(m_architecture);

            foreach (var model in m_architecture.m_iocContainer.EachOfType<IModel>())
            {
                model.Init();
            }

            foreach (var system in m_architecture.m_iocContainer.EachOfType<ISystem>())
            {
                system.Init();
            }

            m_architecture.m_initialized = true;
        }

        public static void DestroyInstance()
        {
            if (m_architecture == null)
            {
                return;
            }

            foreach (var system in m_architecture.m_iocContainer.EachOfType<ISystem>())
            {
                system.Destroy();
            }

            foreach (var model in m_architecture.m_iocContainer.EachOfType<IModel>())
            {
                model.Destroy();
            }

            foreach(var utility in m_architecture.m_iocContainer.EachOfType<IUtility>())
            {
                utility.Destroy();
            }

            m_architecture.m_iocContainer.Clear();
            m_architecture.m_eventSystem.Clear();

            m_architecture.OnDestroy();
            m_architecture = null;
        }

        private readonly IIOCContainer m_iocContainer;
        private readonly TypeEventSystem m_eventSystem = new TypeEventSystem();

        private bool m_initialized = false;
        protected bool Initialized => m_initialized;

        protected Architecture() 
        {
            m_iocContainer = CreateIOCContainer();
        }

        public virtual void RegisterModel<TModel>(TModel model) where TModel : class, IModel
        {
            if (m_iocContainer.TryGet<TModel>(out var removed))
            {
                m_iocContainer.UnRegister<TModel>();
                removed.Destroy();
            }
            
            model.SetArchiecture(this);
            m_iocContainer.Register(model, m_initialized);

            if (m_initialized)
            {
                model.Init();
            }
        }

        public virtual void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            if (m_iocContainer.TryGet<TSystem>(out var removed))
            {
                m_iocContainer.UnRegister<TSystem>();
                removed.Destroy();
            }

            system.SetArchiecture(this);
            m_iocContainer.Register(system, m_initialized);

            if (m_initialized)
            {
                system.Init();
            }
        }

        public virtual void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            if (m_iocContainer.TryGet<TUtility>(out var removed))
            {
                m_iocContainer.UnRegister<TUtility>();
                removed.Destroy();
            }

            utility.SetArchiecture(this);
            m_iocContainer.Register(utility);
        }

        public virtual TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return m_iocContainer.Get<TSystem>();
        }

        public virtual TModel GetModel<TModel>() where TModel : class, IModel
        {
            return m_iocContainer.Get<TModel>();
        }

        public virtual TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return m_iocContainer.Get<TUtility>();
        }

        public virtual void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            using (new ArchitectureWorkspace(this))
            {
                new TCommand().Execute();
            }
        }

        public virtual void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            using (new ArchitectureWorkspace(this))
            {
                command.Execute();
            }
        }

        public virtual TResult SendCommand<TCommand, TResult>() where TCommand : ICommand<TResult>, new()
        {
            using (new ArchitectureWorkspace(this)) 
            {
                return new TCommand().Execute();
            }
        }

        public virtual TResult SendCommand<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            using (new ArchitectureWorkspace(this))
            {
                return command.Execute();
            }
        }

        public virtual TResult SendQuery<TQuery, TResult>() where TQuery : IQuery<TResult>, new()
        {
            using (new ArchitectureWorkspace(this))
            {
                return new TQuery().Do();
            }
        }

        public virtual TResult SendQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            using (new ArchitectureWorkspace(this))
            {
                return query.Do();
            }
        }

        public virtual void SendEvent<TEvent>() where TEvent : new()
        {
            m_eventSystem.Send<TEvent>();
        }

        public virtual void SendEvent<TEvent>(TEvent @event)
        {
            m_eventSystem.Send(@event);
        }

        public virtual IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return m_eventSystem.Register(onEvent);
        }

        public virtual void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            m_eventSystem.UnRegister(onEvent);
        }

        public virtual void Inject<TClass>(TClass instance) where TClass : class
        {
            m_iocContainer.Inject(instance);
        }

        public virtual void Inject<TStruct>(ref TStruct instance) where TStruct : struct
        {
            m_iocContainer.Inject(ref instance);
        }

        protected virtual IIOCContainer CreateIOCContainer()
        {
            return new IOCContainer();
        }

        protected abstract void OnInit();

        protected abstract void OnDestroy();
    }
}