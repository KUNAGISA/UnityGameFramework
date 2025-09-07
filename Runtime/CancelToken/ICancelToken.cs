namespace GameFramework
{
    public interface ICancelToken
    {
        /// <summary>
        /// 取消行为
        /// **取消后Token已经销毁，后续应不再持有**
        /// </summary>
        void Cancel();
    }
}