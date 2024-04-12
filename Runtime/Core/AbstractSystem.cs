﻿namespace Framework
{
    public interface ISystem : IQueryProvider, ICommandProvider, ISetArchitecture, ICanInit, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendCommand, ICanSendQuery, ICanSendEvent, ICanRegisterEvent
    {

    }

    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture m_architecture = null;

        protected virtual void OnInit() { }
        protected virtual void OnDestroy() { }

        void ICanInit.Init() => OnInit();
        void ICanInit.Destroy() => OnDestroy();
        void ISetArchitecture.SetArchitecture(IArchitecture architecture) => m_architecture = architecture;
        IArchitecture IBelongArchitecture.GetArchitecture() => m_architecture;
    }
}