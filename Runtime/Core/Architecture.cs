using Framework.Internals;
using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IArchitecture : IGetModel, IGetSystem, IGetUtility, ISendEvent, ISendCommand, ISendQuery
    {
        void RegisterSystem<T>(T system) where T : class, ISystem;

        void RegisterModel<T>(T model) where T : class, IModel;

        void RegisterUtility<T>(T utility) where T : class, IUtility;

        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        void UnRegisterEvent<T>(Action<T> onEvent);

        void Inject(object @object);
    }

    public abstract class Architecture<T> : IArchitecture, ICommand.IAccess, IQuery.IAccess where T : Architecture<T>, IArchitecture, new()
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

        public static bool IsValid => m_architecture != null;

        public static void MakeSureArchitecture()
        {
            if (m_architecture != null)
            {
                return;
            }

            m_architecture = new T();
            m_architecture.OnInit();

            m_architecture.OnDealInitList(m_architecture.m_initModelList);
            m_architecture.OnDealInitList(m_architecture.m_initSystemList);

            m_architecture.m_init = true;
        }

        public static void DestroyInstance()
        {
            if (m_architecture == null)
            {
                return;
            }

            m_architecture.m_iocContainer.ForEach((IDestory instance) => instance.Destroy());
            m_architecture.OnDestroy();
            m_architecture = null;
        }

        private readonly IOCContainer m_iocContainer = new IOCContainer();
        private readonly TypeEventSystem m_eventSystem = new TypeEventSystem();

        private bool m_init = false;
        private readonly List<IInit> m_initModelList = new List<IInit>();
        private readonly List<IInit> m_initSystemList = new List<IInit>();

        protected Architecture() { }

        public void RegisterModel<TModel>(TModel model) where TModel : class, IModel
        {
            if (m_iocContainer.TryGet<TModel>(out var removed))
            {
                m_iocContainer.UnRegister<TModel>();
                removed.Destroy();
            }
            
            model.SetArchiecture(this);
            m_iocContainer.Register(model);
            OnInitWhenRegister(model, m_initModelList);
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            if (m_iocContainer.TryGet<TSystem>(out var removed))
            {
                m_iocContainer.UnRegister<TSystem>();
                removed.Destroy();
            }

            system.SetArchiecture(this);
            m_iocContainer.Register(system);
            OnInitWhenRegister(system, m_initSystemList);
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            if (m_iocContainer.TryGet<TUtility>(out var removed))
            {
                m_iocContainer.UnRegister<TUtility>();
                removed.Destroy();
            }

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
            new TCommand().Execute(this);
        }

        public virtual void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.Execute(this);
        }

        public void SendQuery<TQuery, TResult>(out TResult result) where TQuery : IQuery<TResult>, new()
        {
            result = new TQuery().Do(this);
        }

        public void SendQuery<TQuery, TResult>(TQuery query, out TResult result) where TQuery : IQuery<TResult>
        {
            result = query.Do(this);
        }

        public void SendEvent<TEvent>() where TEvent : new()
        {
            m_eventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(TEvent @event)
        {
            m_eventSystem.Send(@event);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return m_eventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            m_eventSystem.UnRegister(onEvent);
        }

        public void Inject(object @object)
        {
            m_iocContainer.Inject(@object);
        }

        private void OnInitWhenRegister(IInit instance, IList<IInit> initList)
        {
            if (m_init)
            {
                Inject(instance);
                instance.Init();
            }
            else
            {
                initList.Add(instance);
            }
        }

        private void OnDealInitList(IList<IInit> initList)
        {
            foreach(var instance in initList)
            {
                Inject(instance);
                instance.Init();
            }
            initList.Clear();
        }

        protected abstract void OnInit();

        protected abstract void OnDestroy();
    }
}