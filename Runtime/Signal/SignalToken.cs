namespace GameFramework
{
    public readonly struct SignalToken
    {
        internal readonly ISignal signal;
        internal readonly int index;
        internal readonly int version;
        
        internal SignalToken(ISignal signal, int index, int version)
        {
            this.index = index;
            this.version = version;
            this.signal = signal;
        }

        public void Cancel()
        {
            signal?.Cancel(this);
        }
    }
}