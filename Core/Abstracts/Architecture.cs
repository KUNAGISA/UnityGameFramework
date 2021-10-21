using System.Collections.Generic;
using System;

namespace Framework
{
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
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

        private IOCContainer m_IOCContainer = new IOCContainer();
        private EventSystem m_EventSystem = new EventSystem();

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool mInit = false;

        private List<IModel> m_InitModelList = new List<IModel>();
        private List<ISystem> m_InitSystemList = new List<ISystem>();

        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
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

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
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

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
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
            SendCommand(command);
        }

        public virtual void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.SetArchiecture(this);
            command.Execute();
            command.SetArchiecture(null);
        }

        public virtual void SendEvent<TEvent>() where TEvent : new()
        {
            m_EventSystem.Send<TEvent>();
        }

        public virtual void SendEvent<TEvent>(in TEvent e)
        {
            m_EventSystem.Send(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return m_EventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            m_EventSystem.UnRegister(onEvent);
        }
    }
}