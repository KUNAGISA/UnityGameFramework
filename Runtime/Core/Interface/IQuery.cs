using Framework.Internal.Operate;

namespace Framework
{
    public interface IQueryOperate : IGetModel, IGetSystem, IGetUtility, ISendQuery
    {

    }

    public interface IQuery<TResult>
    {
        TResult Do(IQueryOperate operate);
    }
}
