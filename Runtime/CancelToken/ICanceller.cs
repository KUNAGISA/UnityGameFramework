namespace GameFramework
{
    public interface ICanceller<T>
    {
        void Cancel(T target);
    }
}