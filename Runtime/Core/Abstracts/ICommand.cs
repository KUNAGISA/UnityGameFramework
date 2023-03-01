namespace Framework
{
    public interface ICommand : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendEvent, ICanSendQuery, ICanSendCommand
    {
        internal protected void Execute();

        IArchitecture IBelongArchiecture.GetArchitecture() => ArchitectureWorkspace.ExecutingArchitecture;
    }
}