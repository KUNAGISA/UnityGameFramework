namespace Framework
{
    public interface ISystem : IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanGetModel, ICanRegisterEvent, ICanSendEvent, ICanSendQuery, ICanGetSystem
    {
        void InitSystem();
    }
}