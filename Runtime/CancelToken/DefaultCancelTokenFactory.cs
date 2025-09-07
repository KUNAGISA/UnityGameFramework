namespace GameFramework
{
    internal class DefaultCancelTokenFactory : ICancelTokenFactory
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