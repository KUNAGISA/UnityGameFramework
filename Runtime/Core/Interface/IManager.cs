using Framework.Internal.Operate;

namespace Framework
{
    public interface IManager : IController, ICanSetArchiecture, IDestory
    {
        void InitManager();
    }
}
