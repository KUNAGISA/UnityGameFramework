using Framework.Internals;

namespace Framework
{
    public interface IQuery
    {
        protected internal interface IAccess : IGetSystem, IGetModel, IGetUtility, ISendQuery
        {

        }

        protected internal object Do(IAccess access);
    }

    public interface IQuery<TResult> : IQuery
    {
        new protected internal TResult Do(IAccess architecture);
    }
}
