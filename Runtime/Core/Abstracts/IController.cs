namespace Framework
{
    public interface IController : IBelongArchiecture, ICanGetModel, ICanGetSystem, ICanGetUtility, ICanRegisterEvent, ICanSendCommand, ICanSendQuery, ICanSendEvent
    {
        
    }
}