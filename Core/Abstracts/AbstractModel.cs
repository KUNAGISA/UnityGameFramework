
namespace Framework
{
    public abstract class AbstractModel : IModel
    {
        void IModel.InitMode()
        {
            OnInitModel();
        }
        abstract protected void OnInitModel();

        private IArchitecture m_Architecture;

        IArchitecture IBelongArchiecture.GetArchitecture()
        {
            return m_Architecture;
        }

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture)
        {
            m_Architecture = architecture;
        }
    }
}