### 简单的分层框架

这是一个基于[`QFramework`](https://github.com/liangxiegame/QFramework)再改造的框架，简单的分为`System`，`Model`，`Utility`三层，再加上`Command`和`Query`作为通用功能处理。同层之间理论不允许互相访问，下层不允许访问上层。

- `System`层是模块化的逻辑功能，给多个`Controller`提供共享的逻辑功能，允许访问`System`(但注意不要循环引用)，`Model`，`Utility`，可以执行`Command`和`Query`，可以发送和接收事件。
- `Model`层是数据模块，给`System`层和`Controller`提供共享的数据功能。允许访问`Utility`，可以发送事件。
- `Utility`层是工具模块，比如一些持久化数据，配置读取，SDK之类的就可以放到这一层。可以发送事件
- `Command`跟`System`差不多，但这是一个无状态对象，可以做成`struct`。如果一个功能涉及到多个模块，不知道放哪里的时候就可以直接做成`Command`。允许访问`System`，`Model`，`Utility`，可以执行`Command`和`Query`，可以发送事件。
- `Query`一般用于多模块的组合数据查询。允许访问`System`，`Model`，`Utility`，可以执行`Query`，可以发送事件。

> 关于`Controller`层

`QFramework`中的`Controller`层在这框架中不直接提供，可以每个项目自己定义一个对应的接口，参考如下

```c#
internal interface IController : ICanGetModel, ICanGetSystem, ICanGetUtility, ICanSendCommand, ICanSendEvent, ICanSendQuery, ICanRegisterEvent
{
    IArchitecture IBelongArchitecture.GetArchitecture() => MyArchitecture.Instance;
}
```

> 关于`ICommandProvider`和`IQueryProvider`两个接口

这两个接口分别是`ICommand`和`IQuery`执行接口中传入的对象，`QFramwork`中`Command`和`Query`也是使用扩展接口的方式去处理，但如果是结构体的话会有装箱，所以这里就改成了把`Architecture`作为`Provider`传递进去使用。

> 关于`BindableProperty`

这边使用`IEqualityComparer<T>`接口来做两个数值是否相等的判断，默认情况下使用`EqualityComparer<T>.Default`，如果需要自定义`Comparer`，可以修改`BindableProperty<T>.Comparer`静态属性。
