
namespace Framework
{
    public interface ICommand : IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanGetModel, ICanGetSystem, ICanSendCommand, ICanSendEvent
    {
        void Execute();
    }
}