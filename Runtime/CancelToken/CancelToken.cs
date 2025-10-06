using System.Threading;

namespace GameFramework
{
    public static class CancelToken
    {
        private static ICancelTokenFactory _factory = new DefaultCancelTokenFactory();
        public static ICancelTokenFactory Factory
        {
            set => _factory = value ?? new DefaultCancelTokenFactory();
        }

        private sealed class NoneCancelToken : ICancelToken
        {
            public void Cancel() { }
        }
        
        public static ICancelToken None { get; } = new NoneCancelToken();
        
        public static CancelToken<T> Get<T>(ICanceller<T> canceller, T target)
        {
            var token = _factory.Create<T>();
            token.Reset(canceller, target);
            return token;
        }

        public static void Release<T>(CancelToken<T> token)
        {
            token.Reset(null, default);
            _factory.Recycle(token);
        }
    }

    public sealed class CancelToken<T> : ICancelToken
    {
        private ICanceller<T> _canceller = null;
        private T _target = default;
        
        public void Reset(ICanceller<T> canceller, T target)
        {
            _canceller = canceller;
            _target = target;
        }
        
        public void Cancel()
        {
            _canceller?.Cancel(_target);
            _canceller = null; _target = default;
            CancelToken.Release(this);
        }
    }
}