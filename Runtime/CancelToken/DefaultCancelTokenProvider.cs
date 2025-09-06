namespace GameFramework
{
    internal class DefaultCancelTokenProvider : ICancelTokenProvider
    {
        public CancelToken<T> Create<T>()
        {
            return new CancelToken<T>();
        }

        public void Recycle<T>(CancelToken<T> token)
        {
            
        }
    }
}