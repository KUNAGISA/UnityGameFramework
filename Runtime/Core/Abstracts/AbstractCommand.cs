namespace Framework
{
    public interface ICommand : ICanGetModel, ICanGetSystem, ICanGetUtility, ICanSendCommand, ICanSendQuery, ICanSendEvent
    {
        internal IArchitecture architecture { get; set; }

        IArchitecture IBelongArchiecture.GetArchitecture() => architecture;

        protected internal void Execute();
    }

    /// <summary>
    /// 提供一个class的Command，也可以直接继承ICommand做个struct的
    /// </summary>
    public abstract class AbstractCommand : ICommand
    {
        IArchitecture ICommand.architecture { get; set; }

        void ICommand.Execute() => Execute();

        protected abstract void Execute();
    }
}
