
namespace Framework
{
    public abstract class AbstractUtility : IUtility, ICanGetUtility
    {
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