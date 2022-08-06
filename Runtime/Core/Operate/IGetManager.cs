namespace Framework.Internal.Operate
{
    public interface IGetManager
    {
        /// <summary>
        /// 获取Manager
        /// </summary>
        /// <typeparam name="T">Manager Class</typeparam>
        /// <returns>Instance</returns>
        public T GetManager<T>() where T : class, IManager;
    }
}
