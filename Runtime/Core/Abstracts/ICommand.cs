namespace Framework
{
    public interface ICommand : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendEvent, ICanSendQuery, ICanSendCommand
    {
        internal IArchitecture Architecture { get; set; }

        internal protected void Execute();

        IArchitecture IBelongArchiecture.GetArchitecture() => Architecture;
    }
}