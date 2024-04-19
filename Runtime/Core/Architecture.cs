using System;

namespace Framework
{
    public interface IArchitecture
    {
        void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility;
        void UnRegisterUtility<TUtility>() where TUtility : class, IUtility;
        TUtility GetUtility<TUtility>() where TUtility : class, IUtility;

        void RegisterModel<TModel>(TModel model) where TModel : class, IModel;
        void UnRegisterModel<TModel>() where TModel : class, IModel;
        TModel GetModel<TModel>()where TModel : class, IModel;

        void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem;
        void UnRegisterSystem<TSystem>() where TSystem : class, ISystem;
        TSystem GetSystem<TSystem>() where TSystem : class, ISystem;

        void SendCommand<TCommand>(TCommand command) where TCommand : ICommand;

        TResult SendCommand<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;

        TResult SendQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;

        IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent);
        void UnRegisterEvent<TEvent>(Action<TEvent> onEvent);

        void SendEvent<TEvent>(TEvent @event);
    }

    public abstract class Architecture<TArchitecture> : IArchitecture, ICommandContext, IQueryContext where TArchitecture : Architecture<TArchitecture>, new()
    {
        private static TArchitecture m_architecture = null;
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

        public static void MakeSureArchitecture()
        {
            if (m_architecture != null)
            {
                return;
            }

            m_architecture = new TArchitecture();
            m_architecture.OnInit();

            OnRegisterPatch?.Invoke(m_architecture);

            foreach(var utility in m_architecture.m_iocContainer.Select<IUtility>())
            {
                utility.Init();
            }
            foreach (var model in m_architecture.m_iocContainer.Select<IModel>())
            {
                model.Init();
            }
            foreach (var system in m_architecture.m_iocContainer.Select<ISystem>())
            {
                system.Init();
            }

            m_architecture.m_initialize = true;
        }

        public static void DestroyInstance()
        {
            if (m_architecture == null)
            {
                return;
            }

            foreach (var system in m_architecture.m_iocContainer.Select<ISystem>())
            {
                system.Destroy();
                system.SetArchitecture(null);
            }
            foreach (var model in m_architecture.m_iocContainer.Select<IModel>())
            {
                model.Destroy();
                model.SetArchitecture(null);
            }
            foreach (var utility in m_architecture.m_iocContainer.Select<IUtility>())
            {
                utility.Destroy();
                utility.SetArchitecture(null);
            }

            m_architecture.m_iocContainer.Clear();
            m_architecture.m_eventSystem.Clear();

            m_architecture.OnDestroy();
            m_architecture = null;
        }

        public static event Action<TArchitecture> OnRegisterPatch;

        private bool m_initialize = false;
        private readonly TypeEventSystem m_eventSystem = new TypeEventSystem();
        private readonly IOCContainer m_iocContainer = new IOCContainer();

        protected abstract void OnInit();
        protected abstract void OnDestroy();

        IArchitecture IBelongArchitecture.GetArchitecture() => this;

        protected bool Contains<T>() where T : class
        {
            return m_iocContainer.Contains<T>();
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            UnRegisterUtility<TUtility>();

            utility.SetArchitecture(this);
            m_iocContainer.Register(utility);

            if (m_initialize)
            {
                utility.Init();
            }
        }

        public void UnRegisterUtility<TUtility>() where TUtility : class, IUtility
        {
            if (m_iocContainer.UnRegister<TUtility>(out var utility))
            {
                utility.Destroy();
                utility.SetArchitecture(null);
            }
        }

        public virtual TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return m_iocContainer.Get<TUtility>();
        }

        public void RegisterModel<TModel>(TModel model) where TModel : class, IModel
        {
            UnRegisterModel<TModel>();

            model.SetArchitecture(this);
            m_iocContainer.Register(model);

            if (m_initialize)
            {
                model.Init();
            }
        }

        public void UnRegisterModel<TModel>() where TModel : class, IModel
        {
            if (m_iocContainer.UnRegister<TModel>(out var model))
            {
                model.Destroy();
                model.SetArchitecture(null);
            }
        }

        public virtual TModel GetModel<TModel>() where TModel : class, IModel
        {
            return m_iocContainer.Get<TModel>();
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            UnRegisterSystem<TSystem>();

            system.SetArchitecture(this);
            m_iocContainer.Register(system);

            if (m_initialize)
            {
                system.Init();
            }
        }

        public void UnRegisterSystem<TSystem>() where TSystem : class, ISystem
        {
            if (m_iocContainer.UnRegister<TSystem>(out var system))
            {
                system.Destroy();
                system.SetArchitecture(null);
            }
        }

        public virtual TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return m_iocContainer.Get<TSystem>();
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

        public virtual IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return m_eventSystem.Register(onEvent);
        }

        public virtual void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            m_eventSystem.UnRegister(onEvent);
        }

        public virtual void SendEvent<TEvent>(TEvent @event)
        {
            m_eventSystem.Send(@event);
        }
    }
}