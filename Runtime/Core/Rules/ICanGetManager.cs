namespace Framework
{
    public interface ICanGetManager : IBelongArchiecture
    {
    }

    public static class CanGetManagerExection
    { 
        public static TManager GetManager<TManager>(this ICanGetManager self) where TManager : class, IManager
        {
            return self.GetArchitecture().GetManager<TManager>();
        }
    }
}
