﻿namespace Framework
{
    public interface IQuery<TResult> : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendQuery
    {
        protected internal IArchitecture ExecutingArchitecture { get; set; }

        protected internal TResult Do();

        IArchitecture IBelongArchiecture.GetArchitecture() => ExecutingArchitecture;
    }
}