using Framework.Internals;

namespace Framework
{
    public interface ICommandArchiecture : IGetManager, IGetModel, IGetSystem, IGetUtility, ISendEvent, ISendCommand, ISendQuery
    {

    }

    public interface ICommand
    {
        void Execute(ICommandArchiecture archiecture);
    }
}