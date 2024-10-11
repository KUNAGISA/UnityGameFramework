using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Framework
{
    public interface IUnRegister : IDisposable
    {

    }

    public interface IUnRegisterable<T>
    {
        void UnRegister(T target);
    }

    public sealed class UnRegister<T> : IUnRegister
    {
        private IUnRegisterable<T> m_unregister = null;
        private T m_target = default;

        public UnRegister(IUnRegisterable<T> unRegister, T target)
        {
            m_unregister = unRegister;
            m_target = target;
        }

        public void Dispose()
        {
            m_unregister?.UnRegister(m_target);
            m_unregister = null; m_target = default;
        }
    }

    public static class UnRegisterExtensions
    {
        internal class UnRegisterTrigger : UnityEngine.MonoBehaviour
        {
            public readonly HashSet<IUnRegister> m_unRegisters = new HashSet<IUnRegister>();

            private void Awake() => hideFlags = UnityEngine.HideFlags.HideAndDontSave;

            public void Add(IUnRegister register) => m_unRegisters.Add(register);

            public void UnRegisterAll()
            {
                foreach (var unregister in m_unRegisters)
                {
                    unregister.Dispose();
                }
                m_unRegisters.Clear();
            }
        }

        internal class UnRegisterOnDestroyTrigger : UnRegisterTrigger
        {
            private void OnDestroy() => UnRegisterAll();
        }

        internal class UnRegisterOnDisableTrigger : UnRegisterTrigger
        {
            private void OnDisable() => UnRegisterAll();
        }

        internal class UnRegisterCurrentSceneUnloadedTrigger : UnRegisterTrigger
        {
            private static UnRegisterCurrentSceneUnloadedTrigger s_default = null;
            public static UnRegisterCurrentSceneUnloadedTrigger Default
            {
                get
                {
                    if (!s_default)
                    {
                        s_default = new UnityEngine.GameObject("[UnRegisterCurrentSceneUnloadedTrigger]")
                            .AddComponent<UnRegisterCurrentSceneUnloadedTrigger>();
                    }
                    return s_default;
                }
            }

            private void Awake()
            {
                DontDestroyOnLoad(this);
                gameObject.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
            }

            private void OnDestroy() => SceneManager.sceneUnloaded -= OnSceneUnloaded;

            private void OnSceneUnloaded(Scene scene) => UnRegisterAll();
        }

        private static readonly List<UnRegisterTrigger> s_caches = new List<UnRegisterTrigger>(2);

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

        public static void UnRegisterWhenCurrentSceneUnloaded(this IUnRegister self)
        {
            UnRegisterCurrentSceneUnloadedTrigger.Default.Add(self);
        }

        public static void TriggerAllUnRegister(this UnityEngine.GameObject self)
        {
            self.GetComponents(s_caches);
            for (var index = 0; index < s_caches.Count; index++)
            {
                s_caches[index].UnRegisterAll();
            }
        }
    }
}
