using Framework.Internals;
using System;

namespace Framework
{
    public interface IArchitecture : ISystem.IContext, IModel.IContext, IUtility.IContext, ICommand.IContext, IQuery.IContext
    {
        void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem;
        void RegisterModel<TModel>(TModel model) where TModel : class, IModel;
        void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility;
        void UnRegisterSystem<TSystem>() where TSystem : class, ISystem;
        void UnRegisterModel<TModel>() where TModel : class, IModel;
        void UnRegisterUtility<TUtility>() where TUtility : class, IUtility;
        void Inject<TClass>(TClass instance) where TClass : class;
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

        public static IArchitecture WeekInstance
        {
            get => m_architecture;
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

            foreach (var utility in m_architecture.m_iocContainer.EachOfType<IUtility>())
            {
                utility.Init();
            }

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

        public virtual void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            if (m_iocContainer.TryGet<TSystem>(out var removed))
            {
                m_iocContainer.UnRegister<TSystem>();
                removed.Destroy();
            }

            UnRegisterSystem<TSystem>();

            system.SetContext(this);
            m_iocContainer.Register(system, m_initialized);

            if (m_initialized)
            {
                system.Init();
            }
        }

        public virtual void RegisterModel<TModel>(TModel model) where TModel : class, IModel
        {
            if (m_iocContainer.TryGet<TModel>(out var removed))
            {
                m_iocContainer.UnRegister<TModel>();
                removed.Destroy();
            }
            
            UnRegisterModel<TModel>();

            model.SetContext(this);
            m_iocContainer.Register(model, m_initialized);

            if (m_initialized)
            {
                model.Init();
            }
        }

        public virtual void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            if (m_iocContainer.TryGet<TUtility>(out var removed))
            {
                m_iocContainer.UnRegister<TUtility>();
                removed.Destroy();
            }

            UnRegisterUtility<TUtility>();

            utility.SetContext(this);
            m_iocContainer.Register(utility);

            if (m_initialized) 
            {
                utility.Init();
            }
        }

        public virtual void UnRegisterSystem<TSystem>() where TSystem : class, ISystem
        {
            if (m_iocContainer.UnRegister<TSystem>(out var system))
            {
                system.Destroy();
                system.SetContext(null);
            }
        }

        public virtual void UnRegisterModel<TModel>() where TModel : class, IModel
        {
            if (m_iocContainer.UnRegister<TModel>(out var model))
            {
                model.Destroy();
                model.SetContext(null);
            }
        }

        public virtual void UnRegisterUtility<TUtility>() where TUtility : class, IUtility
        {
            if (m_iocContainer.UnRegister<TUtility>(out var utility))
            {
                utility.Destroy();
                utility.SetContext(null);
            }
        }

        public virtual void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            new TCommand().Execute(this);
        }

        public virtual void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.Execute(this);
        }

        public virtual TResult SendCommand<TResult, TCommand>() where TCommand : ICommand<TResult>, new()
        {
            return new TCommand().Execute(this);
        }

        public virtual TResult SendCommand<TResult, TCommand>(TCommand command) where TCommand : ICommand<TResult>
        {
            return command.Execute(this);
        }

        public virtual TResult SendQuery<TResult, TQuery>() where TQuery : IQuery<TResult>, new()
        {
            return new TQuery().Do(this);
        }

        public virtual TResult SendQuery<TResult, TQuery>(TQuery query) where TQuery : IQuery<TResult>
        {
            return query.Do(this);
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

        TSystem IContainer<ISystem>.Get<TSystem>() => GetSystem<TSystem>();

        TModel IContainer<IModel>.Get<TModel>() => GetModel<TModel>();

        TUtility IContainer<IUtility>.Get<TUtility>() => GetUtility<TUtility>();

        protected virtual IIOCContainer CreateIOCContainer()
        {
            return new IOCContainer();
        }

        protected virtual TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return m_iocContainer.Get<TSystem>();
        }

        protected virtual TModel GetModel<TModel>() where TModel : class, IModel
        {
            return m_iocContainer.Get<TModel>();
        }

        protected virtual TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return m_iocContainer.Get<TUtility>();
        }

        protected abstract void OnInit();

        protected abstract void OnDestroy();
    }
}