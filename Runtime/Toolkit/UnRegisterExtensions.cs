using System.Collections.Generic;

namespace Framework
{
#if UNITY_2017_1_OR_NEWER
    internal class UnRegisterTrigger : UnityEngine.MonoBehaviour
    {
        public readonly HashSet<IUnRegister> m_unregisters = new HashSet<IUnRegister>();

        public UnRegisterTrigger()
        {
            hideFlags = UnityEngine.HideFlags.HideAndDontSave;
        }

        public void Add(IUnRegister register)
        {
            m_unregisters.Add(register);
        }

        public void UnRegisterAll()
        {
            foreach(var unregister in m_unregisters)
            {
                unregister.UnRegister();
            }
            m_unregisters.Clear();
        }
    }

    internal class UnRegisterOnDestroyTrigger : UnRegisterTrigger
    {
        private void OnDestroy()
        {
            UnRegisterAll();
        }
    }

    internal class UnRegisterOnDisableTrigger : UnRegisterTrigger
    {
        private void OnDisable()
        {
            UnRegisterAll();
        }
    }
#endif

    public static class UnRegisterExtensions
    {
#if UNITY_2017_1_OR_NEWER
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister self, UnityEngine.GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<UnRegisterOnDestroyTrigger>(out var trigger))
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }
            trigger.Add(self);
        }

        public static void UnRegisterWhenGameObjectDisabled(this IUnRegister self, UnityEngine.GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<UnRegisterOnDisableTrigger>(out var trigger))
            {
                trigger = gameObject.AddComponent<UnRegisterOnDisableTrigger>();
            }
            trigger.Add(self);
        }
#endif
    }
}
