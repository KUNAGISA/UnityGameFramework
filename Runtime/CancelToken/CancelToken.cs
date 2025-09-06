namespace GameFramework
{
    public static class CancelToken
    {
        private static ICancelTokenProvider _provider = new DefaultCancelTokenProvider();
        public static ICancelTokenProvider Provider
        {
            set => _provider = value ?? new DefaultCancelTokenProvider();
        }

        public static CancelToken<T> Get<T>(ICanceller<T> canceller, T target)
        {
            var token = _provider.Create<T>();
            token.IsRecyclable = true;
            token.Reset(canceller, target);
            return token;
        }

        public static void Recycle<T>(CancelToken<T> token)
        {
            token.Reset(null, default);
            _provider.Recycle(token);
        }
    }
    
    public sealed class CancelToken<T> : ICancelToken
    {
        public bool IsRecyclable { get; set; } = false;
        
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

            if (IsRecyclable)
            {
                CancelToken.Recycle(this);
            }
        }
    }
}