using System;
using System.Collections.Generic;

namespace GameFramework
{
    public sealed class BindableProperty<T> : IBindableProperty<T>, IReadonlyBindableProperty<T>
    {
        /// <summary>
        /// 获取或设置用于比较两个 T 类型值是否相等的比较器。默认使用 EqualityComparer&lt;T&gt;.Default。
        /// 该属性允许用户自定义如何比较两个 T 类型的实例，这对于需要特殊比较逻辑的情况非常有用。
        /// 当设置新的 Value 时，此比较器将被用来决定是否触发值改变事件。
        /// </summary>
        /// <typeparam name="T">泛型类型参数。</typeparam>
        public static IEqualityComparer<T> Comparer { get; set; } = EqualityComparer<T>.Default;

        private T _value = default;
        public T Value
        {
            get => _value;
            set
            {
                if (!Comparer.Equals(value, _value))
                {
                    _value = value;
                    _onValueChanged?.Emit(_value);
                }
            }
        }

        private Signal<T> _onValueChanged;

        public BindableProperty(T value = default)
        {
            _value = value;
        }

        public void SetValueSilently(T value)
        {
            _value = value;
        }

        public SignalToken Register(Action<T> onValueChanged)
        {
            _onValueChanged ??= new Signal<T>();
            return _onValueChanged.Register(onValueChanged);
        }

        public SignalToken RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(_value);
            return Register(onValueChanged);
        }
        
        public static implicit operator T (BindableProperty<T> property)
        {
            return property._value;
        }
    }
}