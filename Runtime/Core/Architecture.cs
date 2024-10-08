﻿using System;

namespace Framework
{
    public interface IArchitecture
    {
        bool Contains<T>() where T : class;
        void Register<T>(T instance) where T : class, ISetArchitecture, IInitializable;
        void UnRegister<T>() where T : class, ISetArchitecture, IInitializable;
        T Get<T>() where T : class;

        void SendCommand<TCommand>(TCommand command) where TCommand : ICommand;

        TResult SendCommand<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;

        TResult SendQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;

        IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent);
        void UnRegisterEvent<TEvent>(Action<TEvent> onEvent);

        void SendEvent<TEvent>(TEvent @event);
    }

    public abstract class Architecture<TArchitecture> : IArchitecture, ICommandContext, IQueryContext where TArchitecture : Architecture<TArchitecture>, new()
    {
        public static event Action<TArchitecture> OnRegisterPatch;

        private static TArchitecture m_architecture = null;
        public static TArchitecture Instance
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

        private bool m_initialize = false;
        private readonly TypeEventSystem m_eventSystem = new TypeEventSystem();
        private readonly IOCContainer m_iocContainer = new IOCContainer();

        protected abstract void OnInit();
        protected abstract void OnDestroy();

        IArchitecture IBelongArchitecture.GetArchitecture() => this;

        public bool Contains<T>() where T : class
        {
            return m_iocContainer.Contains<T>();
        }

        public void Register<T>(T instance) where T : class, ISetArchitecture, IInitializable
        {
            UnRegister<T>();

            instance.SetArchitecture(this);
            m_iocContainer.Register(instance);

            if (m_initialize)
            {
                instance.Init();
            }
        }

        public void UnRegister<T>() where T : class, ISetArchitecture, IInitializable
        {
            if (m_iocContainer.UnRegister<T>(out var instance))
            {
                instance.Destroy();
                instance.SetArchitecture(null);
            }
        }

        public virtual T Get<T>() where T : class
        {
            return m_iocContainer.Get<T>();
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