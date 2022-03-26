
namespace Framework
{
    public interface ICanGetModel : IBelongArchiecture
    {
    }

    public static class CanGetModelExtension
    {
        static public T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }    
    }
}
