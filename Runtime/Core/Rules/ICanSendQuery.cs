namespace Framework
{
    public interface ICanSendQuery : IBelongArchiecture
    {
    }

    public static class CanSendQueryExtension
    {
        static public TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query)
        {
            return self.GetArchitecture().SendQuery(query);
        }
    }
}
