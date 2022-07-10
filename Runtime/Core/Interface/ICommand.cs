using Framework.Internal.Operate;

namespace Framework
{
    public interface ICommandArchiecture : IGetModel, IGetSystem, IGetUtility, ISendCommand, ISendQuery, ISendEvent
    {

    }

    public interface ICommand
    {
        void Execute(ICommandArchiecture archiecture);
    }
}