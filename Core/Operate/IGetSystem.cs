namespace Framework.Internal.Operate
{
    public interface IGetSystem
    {
        /// <summary>
        /// 获取系统逻辑
        /// </summary>
        T GetSystem<T>() where T : class, ISystem;
    }
}
