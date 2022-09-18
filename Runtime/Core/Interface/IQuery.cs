using Framework.Internals;

namespace Framework
{
    public interface IQuery
    {
        protected internal interface IAccess : IGetSystem, IGetModel, IGetUtility, ISendQuery
        {

        }
    }

    public interface IQuery<TResult> : IQuery
    {
        protected internal TResult Do(IAccess architecture);
    }
}
