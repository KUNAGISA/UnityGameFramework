
namespace Framework
{
    public abstract class AbstractModel : IModel
    {
        void IModel.InitMode()
        {
            OnInitModel();
        }
        abstract protected void OnInitModel();

        private IArchitecture mArchitecture;

        IArchitecture IBelongArchiecture.GetArchitecture()
        {
            return mArchitecture;
        }

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
}