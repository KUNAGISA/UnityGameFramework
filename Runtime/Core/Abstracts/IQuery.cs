namespace Framework
{
    public interface IQuery<TResult> : ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendQuery
    {
        internal IArchitecture Architecture { get; set; }

        protected internal TResult Do();

        IArchitecture IBelongArchiecture.GetArchitecture() => Architecture;
    }
}