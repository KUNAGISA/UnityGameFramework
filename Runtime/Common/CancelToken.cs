using System.Collections.Generic;

namespace GameFramework
{
    public interface ICancelToken
    {
        void Cancel();
    }

    public interface ICanceller<T>
    {
        void Cancel(T target);
    }

    public sealed class CancelToken<T> : ICancelToken
    {
        private ICanceller<T> _canceller = null;
        private T _target = default;

        public CancelToken(ICanceller<T> unRegister, T target)
        {
            _canceller = unRegister;
            _target = target;
        }

        public void Cancel()
        {
            _canceller?.Cancel(_target);
            _canceller = null; _target = default;
        }
    }

    public static class CancelTokenExtensions
    {
        internal class CancelTokenTrigger : UnityEngine.MonoBehaviour
        {
            private readonly HashSet<ICancelToken> _cancels = new HashSet<ICancelToken>();

            private void Awake() => hideFlags = UnityEngine.HideFlags.HideAndDontSave;

            public void Add(ICancelToken token) => _cancels.Add(token);

            public void CancelAll()
            {
                foreach (var unregister in _cancels)
                {
                    unregister.Cancel();
                }
                _cancels.Clear();
            }
        }

        [UnityEngine.DisallowMultipleComponent]
        internal class CancelOnDestroyTrigger : CancelTokenTrigger
        {
            private void OnDestroy() => CancelAll();
        }

        [UnityEngine.DisallowMultipleComponent]
        internal class CancelOnDisableTrigger : CancelTokenTrigger
        {
            private void OnDisable() => CancelAll();
        }
        
        private static readonly List<CancelTokenTrigger> s_caches = new List<CancelTokenTrigger>(2);

        public static void CancelWhenGameObjectDestroyed(this ICancelToken self, UnityEngine.GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<CancelOnDestroyTrigger>(out var trigger))
            {
                trigger = gameObject.AddComponent<CancelOnDestroyTrigger>();
            }
            trigger.Add(self);
        }

        public static void CancelWhenGameObjectDisabled(this ICancelToken self, UnityEngine.GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<CancelOnDisableTrigger>(out var trigger))
            {
                trigger = gameObject.AddComponent<CancelOnDisableTrigger>();
            }
            trigger.Add(self);
        }
        
        public static void CancelAllCancelToken(this UnityEngine.GameObject self)
        {
            if (self.TryGetComponent<CancelOnDestroyTrigger>(out var cancelOnDestroyTrigger))
            {
                cancelOnDestroyTrigger.CancelAll();
            }
            if (self.TryGetComponent<CancelOnDisableTrigger>(out var cancelOnDisableTrigger))
            {
                cancelOnDisableTrigger.CancelAll();
            }
        }
    }
}
