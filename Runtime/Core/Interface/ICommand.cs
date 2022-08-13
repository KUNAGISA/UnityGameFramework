using Framework.Internals;

namespace Framework
{
    public interface ICommandArchiecture : IGetModel, IGetSystem, IGetUtility, ISendEvent, ISendCommand, ISendQuery
    {

    }

    public interface ICommand
    {
        void Execute(ICommandArchiecture archiecture);
    }
}