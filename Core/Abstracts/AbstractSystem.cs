
namespace Framework
{
    public abstract class AbstractSystem : ISystem
    {
        void ISystem.InitSystem()
        {
            OnInitSystem();
        }
        abstract protected void OnInitSystem();

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