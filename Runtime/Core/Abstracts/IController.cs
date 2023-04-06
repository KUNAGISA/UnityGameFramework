namespace Framework
{
    public interface IController : ICanGetModel, ICanGetSystem, ICanGetUtility, ICanRegisterEvent, ICanSendCommand, ICanSendQuery, ICanSendEvent, ICanInject
    {
        
    }
}