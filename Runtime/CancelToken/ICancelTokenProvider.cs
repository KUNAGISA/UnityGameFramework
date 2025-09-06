namespace GameFramework
{
    public interface ICancelTokenProvider
    {
        CancelToken<T> Create<T>();
        void Recycle<T>(CancelToken<T> token);
    }
}