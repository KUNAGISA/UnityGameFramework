
namespace Framework
{
    public abstract class AbstractCommand : ICommand
    {
        void ICommand.Execute()
        {
            OnExecute();
        }

        abstract protected void OnExecute();

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