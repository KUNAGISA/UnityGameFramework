### 简单的分层框架

这是一个基于[`QFramework`](https://github.com/liangxiegame/QFramework)再改造的框架，简单的分为`System`，`Model`，`Utility`三层，再加上`Command`和`Query`作为通用功能处理。t同层之间理论不允许互相访问，下层不允许访问上层。

- `System`层是模块化的逻辑功能，给多个`Controller`提供共享的逻辑功能，允许访问`System`(但注意不要循环引用)，`Model`，`Utility`，可以执行`Command`和`Query`，可以发送和接收事件。
- `Model`层是数据模块，给`System`层和`Controller`提供共享的数据功能。允许访问`Utility`，可以发送事件。
- `Utility`层是工具模块，比如一些持久化数据，配置读取，SDK之类的就可以放到这一层。可以发送事件
- `Command`跟`System`差不多，但这是一个无状态对象，可以做成`struct`，或者使用`using Shared<T>.Get(out var result)`减少GC压力。如果一个功能涉及到多个模块，不知道放哪里的时候就可以直接做成`Command`。允许访问`System`，`Model`，`Utility`，可以执行`Command`和`Query`，可以发送事件。
- `Query`一般用于多模块的组合数据查询。允许访问`System`，`Model`，`Utility`，可以执行`Query`，可以发送事件。

> 关于`Controller`层

`QFramework`中的`Controller`层在这里干掉了，相对的，`Controller`可以直接访问`Architecture`对象。也可以考虑如下处理

```c#
using UnityEngine;
using Framework;

public abstract class ArchitectureMono<T> : Monobehaviour where T : Architecture<T>, new()
{
    protected static IArchitecture Context => Architecture<T>.Instance;
}

public abstract class AbstractController<T> where T : Architecture<T>, new()
{
    protected static IArchitecture Context => Architecture<T>.Instance;
}
```

