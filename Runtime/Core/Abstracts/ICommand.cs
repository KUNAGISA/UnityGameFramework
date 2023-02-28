namespace Framework
{
    public interface ICommand : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendEvent, ICanSendQuery, ICanSendCommand
    {
        protected internal IArchitecture ExecutingArchitecture { get; set; }

        internal protected void Execute();

        IArchitecture IBelongArchiecture.GetArchitecture() => ExecutingArchitecture;
    }
}