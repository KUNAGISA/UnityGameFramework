using Framework.Internal.Operate;

namespace Framework
{
    /// <summary>
    /// 数据模块基类
    /// </summary>
    public abstract class AbstractModel : IModel, IDestory
    {
        private IArchitecture m_Architecture;

        void IModel.InitMode() => OnInitModel();

        void IDestory.Destroy() => OnDestoryModel();

        IArchitecture IBelongArchiecture.GetArchitecture() => m_Architecture;

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture) => m_Architecture = architecture;

        abstract protected void OnInitModel();

        abstract protected void OnDestoryModel();
    }
}