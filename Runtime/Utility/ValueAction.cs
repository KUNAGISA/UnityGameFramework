using System;

namespace Framework
{
    public delegate void ValueAction<T>(in T @event) where T : struct;
}
