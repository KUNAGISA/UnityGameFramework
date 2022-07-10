using Framework.Internal.Operate;

namespace Framework
{
    public interface IModel : IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanSendEvent, IDestory
    {
        void InitMode();
    }
}