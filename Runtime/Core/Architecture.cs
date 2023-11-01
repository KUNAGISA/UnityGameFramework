using System;

namespace Framework
{
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
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

        public static void MakeSureArchitecture()
        {
            if (m_architecture != null)
            {
                return;
            }

            m_architecture = new T();
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

            m_architecture.OnDestroy();

            foreach (var system in m_architecture.m_iocContainer.Select<ISystem>())
            {
                system.Destroy();
            }
            foreach (var model in m_architecture.m_iocContainer.Select<IModel>())
            {
                model.Destroy();
            }
            foreach (var utility in m_architecture.m_iocContainer.Select<IUtility>())
            {
                utility.Destroy();
            }

            m_architecture = null;
        }

        public static event Action<T> OnRegisterPatch;

        protected abstract void OnInit();
        protected abstract void OnDestroy();

        private bool m_initialize = false;
        private readonly TypeEventSystem m_eventSystem = new TypeEventSystem();
        private readonly IOCContainer m_iocContainer = new IOCContainer();

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            UnRegisterSystem<TSystem>();

            system.SetContext(this);
            m_iocContainer.Register(system);

            if (m_initialize)
            {
                system.Init();
            }
        }

        public void RegisterModel<TModel>(TModel model) where TModel : class, IModel
        {
            UnRegisterModel<TModel>();

            model.SetContext(this);
            m_iocContainer.Register(model);

            if (m_initialize)
            {
                model.Init();
            }
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            UnRegisterUtility<TUtility>();

            utility.SetContext(this);
            m_iocContainer.Register(utility);

            if (m_initialize)
            {
                utility.Init();
            }
        }

        public void UnRegisterSystem<TSystem>() where TSystem : class, ISystem
        {
            if (m_iocContainer.UnRegister<TSystem>(out var system))
            {
                system.Destroy();
                system.SetContext(null);
            }
        }

        public void UnRegisterModel<TModel>() where TModel : class, IModel
        {
            if (m_iocContainer.UnRegister<TModel>(out var model))
            {
                model.Destroy();
                model.SetContext(null);
            }
        }

        public void UnRegisterUtility<TUtility>() where TUtility : class, IUtility
        {
            if (m_iocContainer.UnRegister<TUtility>(out var utility))
            {
                utility.Destroy();
                utility.SetContext(null);
            }
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return m_iocContainer.Get<TSystem>();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return m_iocContainer.Get<TModel>();
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return m_iocContainer.Get<TUtility>();
        }

        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            new TCommand().Execute(this);
        }

        public void SendCommand<TCommand>(in TCommand command) where TCommand : ICommand
        {
            command.Execute(this);
        }

        public TResult SendCommand<TResult, TCommand>() where TCommand : ICommand<TResult>, new()
        {
            return new TCommand().Execute(this);
        }

        public TResult SendCommand<TResult, TCommand>(in TCommand command) where TCommand : ICommand<TResult>
        {
            return command.Execute(this);
        }

        public TResult SendQuery<TResult, TQuery>() where TQuery : IQuery<TResult>, new()
        {
            return new TQuery().Do(this);
        }

        public TResult SendQuery<TResult, TQuery>(in TQuery query) where TQuery : IQuery<TResult>
        {
            return query.Do(this);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return m_eventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            m_eventSystem.UnRegister(onEvent);
        }

        public void SendEvent<TEvent>() where TEvent : new()
        {
            m_eventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(TEvent @event)
        {
            m_eventSystem.Send(@event);
        }
    }
}