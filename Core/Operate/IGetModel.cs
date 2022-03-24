namespace Framework.Internal.Operate
{
    public interface IGetModel
    {
        /// <summary>
        /// 获取数据模块
        /// </summary>
        public T GetModel<T>() where T : class, IModel;
    }
}
