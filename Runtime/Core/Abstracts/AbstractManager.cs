using Framework.Internal.Operate;

namespace Framework
{
    public abstract class AbstractManager : IManager
    {
        private IArchitecture m_architecure = null;

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecure;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecure = architecture;

        void IManager.InitManager() => OnInitManager();

        void IDestory.Destroy() => OnDestroyManager();

        protected abstract void OnInitManager();

        protected abstract void OnDestroyManager();
    }
}
