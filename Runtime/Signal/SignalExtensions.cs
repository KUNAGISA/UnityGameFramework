namespace GameFramework
{
    public static class SignalExtensions
    {
        public static ICancelToken ToCancelToken(in this SignalToken token)
        {
            return token.signal != null ? CancelToken.Get(token.signal, token) : CancelToken.None;
        }
    }
}