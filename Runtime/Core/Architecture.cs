using Framework.Internal.Operate;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 框架基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Architecture<T> : IArchitecture, ICommandArchiecture, IQueryArchitecture where T : Architecture<T>, IArchitecture, new()
    {
        private static T m_architecture = null;

        static public IArchitecture Instance
        {
            get 
            {
                MakeSureArchitecture();
                return m_architecture;
            }
        }

        static public bool IsValid => m_architecture != null;

        static public void DistoryInstance()
        {
            if (m_architecture == null)
            {
                return;
            }

            m_architecture.m_iocContainer.Each((IDestory instance) => instance.Destroy());
            m_architecture = null;
        }

        static void MakeSureArchitecture()
        {
            if (m_architecture != null)
            {
                return;
            }

            m_architecture = new T();
            m_architecture.Init();

            foreach(var model in m_architecture.m_initModelList)
            {
                m_architecture.Inject(model);
                model.InitMode();
            }
            m_architecture.m_initModelList.Clear();

            foreach(var system in m_architecture.m_initSystemList)
            {
                m_architecture.Inject(system);
                system.InitSystem();
            }
            m_architecture.m_initSystemList.Clear();

            foreach(var manager in m_architecture.m_initManagerList)
            {
                m_architecture.Inject(manager);
                manager.InitManager();
            }
            m_architecture.m_initManagerList.Clear();

            m_architecture.m_init = true;
        }

        private readonly IOCContainer m_iocContainer = new IOCContainer();
        private readonly EventSystem m_eventSystem = new EventSystem();

        private bool m_init = false;

        private readonly List<IModel> m_initModelList = new List<IModel>();
        private readonly List<ISystem> m_initSystemList = new List<ISystem>();
        private readonly List<IManager> m_initManagerList = new List<IManager>();

        public void RegisterManager<TManager>(TManager manager) where TManager : class, IManager
        {
            UnRegisterInstance<TManager>();

            manager.SetArchiecture(this);
            m_iocContainer.Register(manager);

            if (m_init)
            {
                m_iocContainer.Inject(manager);
                manager.InitManager();
            }
            else
            {
                m_initManagerList.Add(manager);
            }
        }

        public void RegisterModel<TModel>(TModel model) where TModel : class, IModel
        {
            UnRegisterInstance<TModel>();

            model.SetArchiecture(this);
            m_iocContainer.Register(model);

            if (m_init)
            {
                m_iocContainer.Inject(model);
                model.InitMode();
            }
            else
            {
                m_initModelList.Add(model);
            }
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            UnRegisterInstance<TSystem>();

            system.SetArchiecture(this);
            m_iocContainer.Register(system);

            if (m_init)
            {
                m_iocContainer.Inject(system);
                system.InitSystem();
            }
            else
            {
                m_initSystemList.Add(system);
            }
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            UnRegisterInstance<TUtility>();
            utility.SetArchiecture(this);
            m_iocContainer.Register(utility);
        }

        public TManager GetManager<TManager>() where TManager : class, IManager
        {
            return m_iocContainer.Get<TManager>();
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

        public virtual void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            SendCommand(new TCommand());
        }

        public virtual void SendCommand<TCommand>(in TCommand command) where TCommand : ICommand
        {
            command.Execute(this);
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            return query.Do(this);
        }

        public virtual void SendEvent<TEvent>() where TEvent : struct
        {
            m_eventSystem.Send<TEvent>();
        }

        public virtual void SendEvent<TEvent>(in TEvent @event) where TEvent : struct
        {
            m_eventSystem.Send(in @event);
        }

        public IUnRegister RegisterEvent<TEvent>(IEventSystem.OnEventHandler<TEvent> onEvent) where TEvent : struct
        {
            return m_eventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<TEvent>(IEventSystem.OnEventHandler<TEvent> onEvent) where TEvent : struct
        {
            m_eventSystem.UnRegister(onEvent);
        }

        void IArchitecture.Inject(object @object)
        {
            m_iocContainer.Inject(@object);
        }

        private void UnRegisterInstance<TInstance>() where TInstance : class
        {
            var instance = m_iocContainer.Get<TInstance>();
            if (instance != null)
            {
                (instance as IDestory)?.Destroy();
                m_iocContainer.UnRegister<TInstance>();
            }
        }

        protected abstract void Init();
    }
}