namespace Framework
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T m_instance = null;
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    MakeSureInstance();
                }
                return m_instance;
            }
        }

        public static bool IsValid => m_instance != null;

        public static void MakeSureInstance()
        {
            if (m_instance != null)
            {
                return;
            }

            m_instance = new T();
            m_instance.OnInit();
        }

        public static void DestroyInstance()
        {
            if (m_instance != null)
            {
                m_instance.OnDestroy();
                m_instance = null;
            }
        }

        protected Singleton() { }

        protected abstract void OnInit();

        protected abstract void OnDestroy();
    }
}
