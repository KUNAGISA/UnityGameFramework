using Framework.Internal.Operate;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 框架基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Architecture<T> : IArchitecture, ICommandOperate, IQueryOperate where T : Architecture<T>, new()
    {
        private static T m_Architecture = null;

        static public IArchitecture Instance
        {
            get 
            {
                MakeSureArchitecture();
                return m_Architecture;
            }
        }

        static public void DistoryInstance()
        {
            if (m_Architecture == null)
            {
                return;
            }

            foreach (var item in m_Architecture.m_IOCContainer)
            {
                (item as IDestory)?.Destroy();
            }
            
            m_Architecture = null;
        }

        static void MakeSureArchitecture()
        {
            if (m_Architecture != null)
                return;

            m_Architecture = new T();
            m_Architecture.Init();

            foreach(var model in m_Architecture.m_InitModelList)
            {
                model.InitMode();
            }
            m_Architecture.m_InitModelList.Clear();

            foreach(var system in m_Architecture.m_InitSystemList)
            {
                system.InitSystem();
            }
            m_Architecture.m_InitSystemList.Clear();

            m_Architecture.mInit = true;
        }

        protected abstract void Init();

        private readonly IOCContainer m_IOCContainer = new IOCContainer();
        private readonly EventSystem m_EventSystem = new EventSystem();

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool mInit = false;

        private readonly List<IModel> m_InitModelList = new List<IModel>();
        private readonly List<ISystem> m_InitSystemList = new List<ISystem>();

        private void UnRegister<TInstance>() where TInstance : class
        {
            var instance = m_IOCContainer.Get<TInstance>();
            if (instance != null)
            {
                (instance as IDestory)?.Destroy();
                m_IOCContainer.UnRegister<TInstance>();
            }
        }

        public void RegisterModel<TModel>(TModel model) where TModel : class, IModel
        {
            UnRegister<TModel>();
            model.SetArchiecture(this);
            m_IOCContainer.Register(model);
            if (mInit)
            {
                model.InitMode();
            }
            else
            {
                m_InitModelList.Add(model);
            }
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            UnRegister<TSystem>();
            system.SetArchiecture(this);
            m_IOCContainer.Register(system);
            if (mInit)
            {
                system.InitSystem();
            }
            else
            {
                m_InitSystemList.Add(system);
            }
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            UnRegister<TUtility>();
            utility.SetArchiecture(this);
            m_IOCContainer.Register(utility);
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return m_IOCContainer.Get<TSystem>();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return m_IOCContainer.Get<TModel>();
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return m_IOCContainer.Get<TUtility>();
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
            m_EventSystem.Send<TEvent>();
        }

        public virtual void SendEvent<TEvent>(in TEvent @event)
        {
            m_EventSystem.Send(in @event);
        }

        public IUnRegister RegisterEvent<TEvent>(IEventSystem.OnEventHandler<TEvent> onEvent)
        {
            return m_EventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<TEvent>(IEventSystem.OnEventHandler<TEvent> onEvent)
        {
            m_EventSystem.UnRegister(onEvent);
        }
    }
}