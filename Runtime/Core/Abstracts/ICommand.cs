namespace Framework
{
    public interface ICommandContext : ICanGetModel, ICanGetSystem, ICanGetUtility, ICanSendCommand, ICanSendQuery, ICanSendEvent
    {

    }

    public interface ICommand : ICanGetModel, ICanGetSystem, ICanGetUtility, ICanSendCommand, ICanSendQuery, ICanSendEvent
    {
        protected internal void Execute(ICommandContext context);
    }
}
