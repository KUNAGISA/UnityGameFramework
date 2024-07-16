using System;
using System.Collections.Generic;

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

    public static class UnityUnRegisterExtensions
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

        public static void TriggerAllUnRegister(this UnityEngine.GameObject self)
        {
            var triggers = self.GetComponents<UnRegisterTrigger>();
            for (var index = 0; index < triggers.Length; index++)
            {
                triggers[index].UnRegisterAll();
            }
        }
    }
}
