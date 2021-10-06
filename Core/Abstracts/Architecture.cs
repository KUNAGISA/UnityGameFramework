using System.Collections.Generic;
using System;

namespace Framework
{
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private static T mArchitecture = null;

        static public IArchitecture Instance
        {
            get 
            {
                MakeSureArchitecture();
                return mArchitecture;
            }
        }

        static void MakeSureArchitecture()
        {
            if (mArchitecture != null)
                return;

            mArchitecture = new T();
            mArchitecture.Init();

            foreach(var model in mArchitecture.mInitModelList)
            {
                model.InitMode();
            }
            mArchitecture.mInitModelList.Clear();

            foreach(var system in mArchitecture.mInitSystemList)
            {
                system.InitSystem();
            }
            mArchitecture.mInitSystemList.Clear();

            mArchitecture.mInit = true;
        }

        protected abstract void Init();

        private IOCContainer mIOCContainer = new IOCContainer();
        private EventSystem mEventSystem = new EventSystem();

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool mInit = false;

        private List<IModel> mInitModelList = new List<IModel>();
        private List<ISystem> mInitSystemList = new List<ISystem>();

        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchiecture(this);
            mIOCContainer.Register(model);
            if (mInit)
            {
                model.InitMode();
            }
            else
            {
                mInitModelList.Add(model);
            }
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchiecture(this);
            mIOCContainer.Register(system);
            if (mInit)
            {
                system.InitSystem();
            }
            else
            {
                mInitSystemList.Add(system);
            }
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            utility.SetArchiecture(this);
            mIOCContainer.Register(utility);
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return mIOCContainer.Get<TSystem>();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return mIOCContainer.Get<TModel>();
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return mIOCContainer.Get<TUtility>();
        }

        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            TCommand command = new TCommand();
            SendCommand(ref command);
        }

        public void SendCommand<TCommand>(ref TCommand command) where TCommand : ICommand
        {
            command.SetArchiecture(this);
            command.Execute();
            command.SetArchiecture(null);
        }

        public void SendEvent<TEvent>() where TEvent : new()
        {
            mEventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(in TEvent e)
        {
            mEventSystem.Send(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return mEventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            mEventSystem.UnRegister(onEvent);
        }
    }
}