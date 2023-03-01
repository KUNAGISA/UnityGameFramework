namespace Framework
{
    public interface IQuery<TResult> : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendQuery
    {
        protected internal TResult Do();

        IArchitecture IBelongArchiecture.GetArchitecture() => ArchitectureWorkspace.ExecutingArchitecture;
    }
}