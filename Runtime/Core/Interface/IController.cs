namespace Framework
{
    public interface IController : IBelongArchiecture, ICanGetModel, ICanGetSystem, ICanGetUtility, ICanSendCommand, ICanRegisterEvent, ICanSendQuery
    {
    }
}