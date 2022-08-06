namespace Framework
{
    public interface ICanGetUtility : IBelongArchiecture
    {
    }

    public static class CanGetUtilityExtension
    {
        public static TUtility GetUtility<TUtility>(this ICanGetUtility self) where TUtility : class, IUtility
        {
            return self.GetArchitecture().GetUtility<TUtility>();
        }
    }
}
