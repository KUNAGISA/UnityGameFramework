namespace Aoiro
{
    public interface ICanceller<T>
    {
        void Cancel(T target);
    }
}