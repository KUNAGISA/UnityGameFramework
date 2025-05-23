﻿using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IArchitecture
    {
        bool Contains<T>() where T : class;
        void Register<T>(T instance) where T : class, IArchitectureModule;
        void UnRegister<T>() where T : class, IArchitectureModule;
        T Get<T>() where T : class, IArchitectureModule;
        IEnumerable<T> Select<T>() where T : class;

        void SendCommand<TCommand>(TCommand command) where TCommand : ICommand;

        TResult SendCommand<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;

        TResult SendQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;

        IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent);
        void UnRegisterEvent<TEvent>(Action<TEvent> onEvent);

        void SendEvent<TEvent>(in TEvent e);
    }

    public abstract class Architecture<TArchitecture> : IArchitecture, ICommandContext, IQueryContext where TArchitecture : Architecture<TArchitecture>, new()
    {
        public static event Action<TArchitecture> OnRegisterPatch;

        private static TArchitecture s_architecture = null;
        public static IArchitecture Instance
        {
            get
            {
                if (s_architecture == null)
                {
                    MakeSureArchitecture();
                }
                return s_architecture;
            }
        }

        public static bool Vaild => s_architecture != null;

        public static void MakeSureArchitecture()
        {
            if (s_architecture != null)
            {
                return;
            }

            s_architecture = new TArchitecture();
            s_architecture.OnInit();

            OnRegisterPatch?.Invoke(s_architecture);

            foreach(var utility in s_architecture.m_iocContainer.Select<IUtility>())
            {
                utility.Init();
            }
            foreach (var model in s_architecture.m_iocContainer.Select<IModel>())
            {
                model.Init();
            }
            foreach (var system in s_architecture.m_iocContainer.Select<ISystem>())
            {
                system.Init();
            }

            s_architecture.m_initialize = true;
        }

        public static void DestroyInstance()
        {
            if (s_architecture == null)
            {
                return;
            }

            foreach (var system in s_architecture.m_iocContainer.Select<ISystem>())
            {
                system.Destroy();
                system.SetArchitecture(null);
            }
            foreach (var model in s_architecture.m_iocContainer.Select<IModel>())
            {
                model.Destroy();
                model.SetArchitecture(null);
            }
            foreach (var utility in s_architecture.m_iocContainer.Select<IUtility>())
            {
                utility.Destroy();
                utility.SetArchitecture(null);
            }

            s_architecture.m_iocContainer.Clear();
            s_architecture.m_eventSystem.Clear();

            s_architecture.OnDestroy();
            s_architecture = null;
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

        public void Register<T>(T instance) where T : class, IArchitectureModule
        {
            UnRegister<T>();

            instance.SetArchitecture(this);
            m_iocContainer.Register(instance);

            if (m_initialize)
            {
                instance.Init();
            }
        }

        public void UnRegister<T>() where T : class, IArchitectureModule
        {
            if (m_iocContainer.UnRegister<T>(out var instance))
            {
                instance.Destroy();
                instance.SetArchitecture(null);
            }
        }

        public virtual T Get<T>() where T : class, IArchitectureModule
        {
            return m_iocContainer.Get<T>();
        }

        public IEnumerable<T> Select<T>() where T : class
        {
            return m_iocContainer.Select<T>();
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

        public virtual void SendEvent<TEvent>(in TEvent e)
        {
            m_eventSystem.Send(e);
        }
    }
}