using Framework.Internals;

namespace Framework
{
    public interface IQueryArchitecture : IGetSystem, IGetModel, IGetUtility, ISendQuery
    {

    }

    public interface IQuery<TResult>
    {
        TResult Do(IQueryArchitecture architecture);
    }
}
