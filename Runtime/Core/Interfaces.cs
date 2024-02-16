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
#if CUSTOM_ARCHITECTURE
        void Init();
        void Destroy();
#else
        protected internal void Init();
        protected internal void Destroy();
#endif
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