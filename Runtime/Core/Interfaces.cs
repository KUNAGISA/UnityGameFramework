namespace Framework
{
    public interface IBelongArchitecture
    {
#if PUBLIC_BELONG_ARCHITECTURE
        IArchitecture GetArchitecture();
#else
        protected internal IArchitecture GetArchitecture();
#endif
    }

    public interface ISetArchitecture
    {
#if CUSTOM_ARCHITECTURE
        void SetArchitecture(IArchitecture architecture);
#else
        protected internal void SetArchitecture(IArchitecture architecture);
#endif
    }

    public interface ICanInit
    {
        protected internal void Init();
        protected internal void Destroy();
    }

    public interface ICanGetUtility : IBelongArchitecture
    {
        
    }

    public interface ICanGetModel : IBelongArchitecture
    {

    }

    public interface ICanGetSystem : IBelongArchitecture
    {

    }

    public interface ICanSendCommand : IBelongArchitecture
    {

    }

    public interface ICanSendQuery : IBelongArchitecture
    {

    }

    public interface ICanRegisterEvent : IBelongArchitecture
    {

    }

    public interface ICanSendEvent : IBelongArchitecture
    {

    }
}