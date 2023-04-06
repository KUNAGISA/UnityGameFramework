namespace Framework
{
    public interface IQuery<out TResult> : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendQuery
    {
        protected internal TResult Do();

        IArchitecture IBelongArchiecture.GetArchitecture() => ArchitectureWorkspace.ExecutingArchitecture;
    }
}