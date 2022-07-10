using Framework.Internal.Operate;

namespace Framework
{
    public interface ISystem : IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanGetModel, ICanRegisterEvent, ICanSendEvent, ICanSendQuery, ICanGetSystem, IDestory
    {
        void InitSystem();
    }
}