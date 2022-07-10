using Framework.Internal.Operate;

namespace Framework
{
    /// <summary>
    /// 数据模块基类
    /// </summary>
    public abstract class AbstractModel : IModel
    {
        private IArchitecture m_architecture;

        void IModel.InitMode() => OnInitModel();

        void IDestory.Destroy() => OnDestoryModel();

        IArchitecture IBelongArchiecture.GetArchitecture() => m_architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_architecture = architecture;

        abstract protected void OnInitModel();

        abstract protected void OnDestoryModel();
    }
}