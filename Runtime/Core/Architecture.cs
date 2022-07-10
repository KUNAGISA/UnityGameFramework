using Framework.Internal.Operate;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 框架基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Architecture<T> : IArchitecture, ICommandArchiecture, IQueryArchitecture where T : Architecture<T>, new()
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
                return;

            m_architecture = new T();
            m_architecture.Init();

            foreach(var model in m_architecture.m_initModelList)
            {
                model.InitMode();
            }
            m_architecture.m_initModelList.Clear();

            foreach(var system in m_architecture.m_initSystemList)
            {
                system.InitSystem();
            }
            m_architecture.m_initSystemList.Clear();

            m_architecture.m_init = true;
        }

        protected abstract void Init();

        private readonly IOCContainer m_iocContainer = new IOCContainer();
        private readonly EventSystem m_eventSystem = new EventSystem();

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool m_init = false;

        private readonly List<IModel> m_initModelList = new List<IModel>();
        private readonly List<ISystem> m_initSystemList = new List<ISystem>();

        private void UnRegister<TInstance>() where TInstance : class
        {
            var instance = m_iocContainer.Get<TInstance>();
            if (instance != null)
            {
                (instance as IDestory)?.Destroy();
                m_iocContainer.UnRegister<TInstance>();
            }
        }

        public void RegisterModel<TModel>(TModel model) where TModel : class, IModel
        {
            UnRegister<TModel>();
            model.SetArchiecture(this);
            m_iocContainer.Register(model);
            if (m_init)
            {
                model.InitMode();
            }
            else
            {
                m_initModelList.Add(model);
            }
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            UnRegister<TSystem>();
            system.SetArchiecture(this);
            m_iocContainer.Register(system);
            if (m_init)
            {
                system.InitSystem();
            }
            else
            {
                m_initSystemList.Add(system);
            }
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            UnRegister<TUtility>();
            utility.SetArchiecture(this);
            m_iocContainer.Register(utility);
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
            TCommand command = new TCommand();
            SendCommand(in command);
        }

        public virtual void SendCommand<TCommand>(in TCommand command) where TCommand : ICommand
        {
            command.Execute(this);
        }

        public TResult SendQuery<TResult>(in IQuery<TResult> query)
        {
            return query.Do(this);
        }

        public virtual void SendEvent<TEvent>() where TEvent : new()
        {
            m_eventSystem.Send<TEvent>();
        }

        public virtual void SendEvent<TEvent>(in TEvent @event)
        {
            m_eventSystem.Send(in @event);
        }

        public IUnRegister RegisterEvent<TEvent>(IEventSystem.OnEventHandler<TEvent> onEvent)
        {
            return m_eventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<TEvent>(IEventSystem.OnEventHandler<TEvent> onEvent)
        {
            m_eventSystem.UnRegister(onEvent);
        }
    }
}