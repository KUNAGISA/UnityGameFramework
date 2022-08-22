using Framework.Internals;
using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IArchitecture : IGetModel, IGetSystem, IGetUtility, ISendEvent, ISendCommand, ISendQuery
    {
        event Action onArchitectureDestroy;

        void RegisterSystem<T>(T system) where T : class, ISystem;

        void RegisterModel<T>(T model) where T : class, IModel;

        void RegisterUtility<T>(T utility) where T : class, IUtility;

        IUnRegister RegisterEvent<T>(ValueAction<T> onEvent) where T : struct;

        void UnRegisterEvent<T>(ValueAction<T> onEvent) where T : struct;

        void Inject(object @object);
    }

    public abstract class Architecture<T> : IArchitecture, ICommand.IAccess, IQuery.IAccess where T : Architecture<T>, IArchitecture, new()
    {
        private static T m_architecture = null;
        public static IArchitecture Instance
        {
            get
            {
                if (m_architecture != null)
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

            m_architecture.onArchitectureDestroy();
            m_architecture.onArchitectureDestroy = null;
            m_architecture.OnDestroy();
            m_architecture = null;
        }

        public event Action onArchitectureDestroy;
        
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
                onArchitectureDestroy -= removed.Destroy;
                removed.Destroy();
            }
            
            model.SetArchiecture(this);
            m_iocContainer.Register(model);
            onArchitectureDestroy += model.Destroy;
            OnInitWhenRegister(model, m_initModelList);
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            if (m_iocContainer.TryGet<TSystem>(out var removed))
            {
                m_iocContainer.UnRegister<TSystem>();
                onArchitectureDestroy -= removed.Destroy;
                removed.Destroy();
            }

            system.SetArchiecture(this);
            m_iocContainer.Register(system);
            onArchitectureDestroy += system.Destroy;
            OnInitWhenRegister(system, m_initSystemList);
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            if (m_iocContainer.TryGet<TUtility>(out var removed))
            {
                m_iocContainer.UnRegister<TUtility>();
                onArchitectureDestroy -= removed.Destroy;
                removed.Destroy();
            }

            utility.SetArchiecture(this);
            onArchitectureDestroy += utility.Destroy;
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

        public virtual void SendCommand<TCommand>() where TCommand : struct, ICommand
        {
            new TCommand().Execute(this);
        }

        public virtual void SendCommand<TCommand>(in TCommand command) where TCommand : struct, ICommand
        {
            command.Execute(this);
        }

        public void SendQuery<TQuery, TResult>(out TResult result) where TQuery : struct, IQuery<TResult>
        {
            result = new TQuery().Do(this);
        }

        public void SendQuery<TQuery, TResult>(in TQuery query, out TResult result) where TQuery : struct, IQuery<TResult>
        {
            result = query.Do(this);
        }

        public void SendEvent<TEvent>() where TEvent : struct
        {
            m_eventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(in TEvent @event) where TEvent : struct
        {
            m_eventSystem.Send(in @event);
        }

        public IUnRegister RegisterEvent<TEvent>(ValueAction<TEvent> onEvent) where TEvent : struct
        {
            return m_eventSystem.Register(onEvent);
        }

        public void UnRegisterEvent<TEvent>(ValueAction<TEvent> onEvent) where TEvent : struct
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