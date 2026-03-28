namespace GameFramework
{
    public readonly struct SignalToken
    {
        internal readonly ISignal Signal;
        internal readonly int Index;
        internal readonly int Version;
        
        internal SignalToken(ISignal signal, int index, int version)
        {
            Index = index;
            Version = version;
            Signal = signal;
        }

        public void Cancel()
        {
            Signal?.Cancel(this);
        }
    }
}