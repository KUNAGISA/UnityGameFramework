namespace Framework
{
    public interface ICommandContext : ICanGetModel, ICanGetSystem, ICanGetUtility, ICanSendCommand, ICanSendQuery, ICanSendEvent
    {

    }

    public interface ICommand
    {
        protected internal void Execute(ICommandContext context);
    }
}
