
namespace Framework
{
    public interface ICanGetSystem : IBelongArchiecture
    {
    }

    public static class CanGetSystemExtension
    {
        public static TSystem GetSystem<TSystem>(this ICanGetSystem self) where TSystem : class, ISystem
        {
            return self.GetArchitecture().GetSystem<TSystem>();
        }
    }
}
