using System;

namespace GameFramework
{
    internal interface ISignal : ICanceller<SignalToken>
    {
        void Cleanup();
    }

    public abstract class AbstractSignal<T> : ISignal where T : Delegate
    {
        private const int InvalidIndex = -1;

        protected struct SignalSlot
        {
            internal int prevIndex;
            internal int nextIndex;
            internal int version;

            internal T callback;
        
            internal void Invalidate()
            {
                ++version;
                callback = null;
            }
        }
        
        private int _count = 0;
        private SignalSlot[] _slots = null;

        private int _emitCursor = InvalidIndex;
        private int _emitEnd = InvalidIndex;

        private int _activeHead = InvalidIndex;
        private int _activeTail = InvalidIndex;

        private int _freeHead = InvalidIndex;

        protected AbstractSignal(int capacity = 8)
        {
            _slots = new SignalSlot[capacity > 0 ? capacity : 4];
        }
        
        public void Cleanup()
        {
            for (var index = 0; index < _count; ++index)
            {
                _slots[index].Invalidate();
                _slots[index].prevIndex = _slots[index].nextIndex = InvalidIndex;
            }
            _count = 0;
            _freeHead = InvalidIndex;
            _activeHead = InvalidIndex;
            _activeTail = InvalidIndex;
            _emitCursor = InvalidIndex;
            _emitEnd = InvalidIndex;
        }
        
        public SignalToken Register(T callback)
        {
            if (callback == null)
            {
                return default;
            }
            
            int index;
            if (_freeHead == InvalidIndex)
            {
                EnsureCapacity();
                index = _count++;
            }
            else
            {
                index = _freeHead;
                _freeHead = _slots[_freeHead].nextIndex;
            }

            ref var slot = ref _slots[index];
            slot.callback = callback;

            if (_activeHead == InvalidIndex)
            {
                _activeHead = index;
                _activeTail = index;
                slot.prevIndex = InvalidIndex;
                slot.nextIndex = InvalidIndex;
            }
            else
            {
                // 能跑进来_activeTail必定是有效的
                _slots[_activeTail].nextIndex = index;
                slot.prevIndex = _activeTail;
                slot.nextIndex = InvalidIndex;
                _activeTail = index;
            }

            return new SignalToken(this, index, slot.version);
        }
        
        public void Cancel(SignalToken token)
        {
            if (token.signal != this || token.index >= _count || token.index < 0)
            {
                return;
            }

            if (_slots[token.index].version != token.version)
            {
                return;
            }

            ref var slot = ref _slots[token.index];
            var prevIndex = slot.prevIndex;
            var nextIndex = slot.nextIndex;

            if (prevIndex != InvalidIndex)
            {
                _slots[prevIndex].nextIndex = nextIndex;
            }
            if (nextIndex != InvalidIndex)
            {
                _slots[nextIndex].prevIndex = prevIndex;
            }
            if (_activeHead == token.index)
            {
                _activeHead = nextIndex;
            }
            if (_activeTail == token.index)
            {
                _activeTail = prevIndex;
            }
            if (_emitCursor == token.index)
            {
                _emitCursor = nextIndex;
            }
            if (_emitEnd == token.index)
            {
                _emitEnd = prevIndex;
            }
            
            slot.Invalidate();
            slot.prevIndex = InvalidIndex;
            slot.nextIndex = _freeHead;
            _freeHead = token.index;
        }
        
        private void EnsureCapacity()
        {
            if (_count == _slots.Length)
            {
                Array.Resize(ref _slots, _slots.Length << 1);
            }
        }

        protected ref readonly SignalSlot GetSlot(int index)
        {
            return ref _slots[index];
        }
        
        protected void BeginEmit()
        {
            if (_activeHead == InvalidIndex)
            {
                _emitCursor = InvalidIndex;
                _emitEnd = InvalidIndex;
                return;
            }

            _emitCursor = _activeHead;
            _emitEnd = _activeTail;
        }

        protected bool MoveNext(out int current)
        {
            current = _emitCursor;
            if (current == InvalidIndex)
            {
                return false;
            }
            _emitCursor = _slots[current].nextIndex;
            return true;
        }

        protected bool IsLast(int current)
        {
            return current == _emitEnd;
        }

        protected void EndEmit()
        {
            _emitCursor = InvalidIndex;
            _emitEnd = InvalidIndex;
        }
    }

    public sealed class Signal : AbstractSignal<Action>
    {
        public void Emit()
        {
            BeginEmit();
            try
            {
                while (MoveNext(out var current))
                {
                    ref readonly var slot = ref GetSlot(current);
                    slot.callback?.Invoke(); //有错误直接抛出，让外部调用者自己捕获
                    if (IsLast(current))
                    {
                        break;
                    }
                }
            }
            finally
            {
                EndEmit();
            }
        }
    }
    
    public sealed class Signal<T> : AbstractSignal<Action<T>>
    {
        public void Emit(in T arg)
        {
            BeginEmit();
            try
            {
                while (MoveNext(out var current))
                {
                    ref readonly var slot = ref GetSlot(current);
                    slot.callback?.Invoke(arg); //有错误直接抛出，让外部调用者自己捕获
                    if (IsLast(current))
                    {
                        break;
                    }
                }
            }
            finally
            {
                EndEmit();
            }
        }
    }
    
    public sealed class Signal<T1, T2> : AbstractSignal<Action<T1, T2>>
    {
        public void Emit(in T1 arg1, in T2 arg2)
        {
            BeginEmit();
            try
            {
                while (MoveNext(out var current))
                {
                    ref readonly var slot = ref GetSlot(current);
                    slot.callback?.Invoke(arg1, arg2); //有错误直接抛出，让外部调用者自己捕获
                    if (IsLast(current))
                    {
                        break;
                    }
                }
            }
            finally
            {
                EndEmit();
            }
        }
    }
}