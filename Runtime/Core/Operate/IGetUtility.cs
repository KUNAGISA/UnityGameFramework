namespace Framework.Internal.Operate
{
    public interface IGetUtility
    {
        /// <summary>
        /// 获取工具
        /// </summary>
        T GetUtility<T>() where T : class, IUtility;
    }
}
