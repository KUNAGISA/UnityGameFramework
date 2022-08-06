
namespace Framework
{
    public interface ICanGetModel : IBelongArchiecture
    {
    }

    public static class CanGetModelExtension
    {
        static public TModel GetModel<TModel>(this ICanGetModel self) where TModel : class, IModel
        {
            return self.GetArchitecture().GetModel<TModel>();
        }    
    }
}
