using Framework.Internal.Operate;

namespace Framework
{
    public interface ICommandOperate : IGetModel, IGetSystem, IGetUtility, ISendCommand, ISendQuery, ISendEvent
    {

    }

    public interface ICommand
    {
        void Execute(ICommandOperate operate);
    }
}