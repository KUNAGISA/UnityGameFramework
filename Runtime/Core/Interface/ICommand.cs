using Framework.Internals;

namespace Framework
{
    public interface ICommand
    {
        protected internal interface IAccess : IGetModel, IGetSystem, IGetUtility, ISendEvent, ISendCommand, ISendQuery
        {

        }

        protected internal void Execute(IAccess access);
    }
}