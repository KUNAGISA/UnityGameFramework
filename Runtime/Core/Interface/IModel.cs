namespace Framework
{
    public interface IModel : IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanSendEvent
    {
        void InitMode();
    }
}