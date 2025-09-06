namespace GameFramework
{
    public interface ICancelToken
    {
        /// <summary>
        /// 是否可以回收，如果是则在<see cref="Cancel"/>之后回收
        /// </summary>
        bool IsRecyclable { get; set; }
        void Cancel();
    }
}