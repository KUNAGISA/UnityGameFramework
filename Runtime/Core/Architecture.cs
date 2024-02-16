﻿using System;

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

        TResult SendCommand<TResult>(ICommand<TResult> command);
        TResult SendCommand<TResult, TCommand>(TCommand command) where TCommand : struct, ICommand<TResult>;

        TResult SendQuery<TResult>(IQuery<TResult> query);
        TResult SendQuery<TResult, TQuery>(TQuery query) where TQuery : struct, IQuery<TResult>;

        IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent);
        void UnRegisterEvent<TEvent>(Action<TEvent> onEvent);

        void SendEvent<TEvent>() where TEvent : new();
        void SendEvent<TEvent>(TEvent @event);
    }

    public abstract class Architecture<T> : IArchitecture, ICommandContext, IQueryContext where T : Architecture<T>, new()
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

            m_architecture = null;
        }

        public static event Action<T> OnRegisterPatch;

        private bool m_initialize = false;
        private readonly TypeEventSystem m_eventSystem = new TypeEventSystem();
        private readonly IOCContainer m_iocContainer = new IOCContainer();

        protected abstract void OnInit();
        protected abstract void OnDestroy();

        IArchitecture IBelongArchitecture.GetArchitecture() => m_architecture;

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

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
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

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return m_iocContainer.Get<TModel>();
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            UnRegisterSystem<TSystem>();
        }

        public void UnRegisterSystem<TSystem>() where TSystem : class, ISystem
        {
            if (m_iocContainer.UnRegister<TSystem>(out var system))
            {
                system.Destroy();
                system.SetArchitecture(null);
            }
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return m_iocContainer.Get<TSystem>();
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.Execute(this);
        }

        public TResult SendCommand<TResult>(ICommand<TResult> command)
        {
            return command.Execute(this);
        }

        public TResult SendCommand<TResult, TCommand>(TCommand command) where TCommand : struct, ICommand<TResult>
        {
            return command.Execute(this);
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            return query.Do(this);
        }

        public TResult SendQuery<TResult, TQuery>(TQuery query) where TQuery : struct, IQuery<TResult>
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