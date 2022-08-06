namespace Framework.Internal.Operate
{
    public interface ISendQuery
    {
        /// <summary>
        /// 发送查询
        /// </summary>
        /// <param name="query">查询指令</param>
        /// <returns>查询结果</returns>
        TResult SendQuery<TResult>(IQuery<TResult> query);
    }
}
