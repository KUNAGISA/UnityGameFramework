namespace GameFramework
{
    public interface ICancelTokenFactory
    {
        CancelToken<T> Create<T>();
        void Recycle<T>(CancelToken<T> token);
    }
}