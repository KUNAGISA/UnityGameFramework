using Framework.Internal.Operate;

namespace Framework
{
    public interface IQueryArchitecture : IGetModel, IGetSystem, IGetUtility, ISendQuery
    {

    }

    public interface IQuery<TResult>
    {
        TResult Do(IQueryArchitecture architecture);
    }
}
