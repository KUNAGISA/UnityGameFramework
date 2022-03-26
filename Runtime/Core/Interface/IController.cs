namespace Framework
{
    public interface IController : IBelongArchiecture, ICanGetModel, ICanGetSystem, ICanSendCommand, ICanRegisterEvent, ICanSendQuery
    {
    }
}