using Framework.Internals;

namespace Framework
{
    public interface IQueryArchitecture : IGetModel, IGetUtility, ISendQuery
    {

    }

    public interface IQuery<TResult>
    {
        TResult Do(IQueryArchitecture architecture);
    }
}
