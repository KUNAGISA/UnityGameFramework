using System;

namespace Framework
{
    public interface IArchitecture
    {
        /// <summary>
        /// 注册或替换新系统模块
        /// </summary>
        /// <typeparam name="T">系统模块</typeparam>
        /// <param name="system">系统模块</param>
        void RegisterSystem<T>(T system) where T : ISystem;

        /// <summary>
        /// 注册或替换数据模块
        /// </summary>
        /// <typeparam name="T">数据模块</typeparam>
        /// <param name="model">数据模块</param>
        void RegisterModel<T>(T model) where T : IModel;

        /// <summary>
        /// 注册或替换工具模块
        /// </summary>
        /// <typeparam name="T">工具模块</typeparam>
        /// <param name="utility">工具模块</param>
        void RegisterUtility<T>(T utility) where T : IUtility;

        /// <summary>
        /// 获取系统模块
        /// </summary>
        /// <typeparam name="T">系统模块</typeparam>
        /// <returns></returns>
        T GetSystem<T>() where T : class, ISystem;

        /// <summary>
        /// 获取数据模块
        /// </summary>
        /// <typeparam name="T">系统模块</typeparam>
        /// <returns></returns>
        T GetModel<T>() where T : class, IModel;

        /// <summary>
        /// 获取工具模块
        /// </summary>
        /// <typeparam name="T">工具模块</typeparam>
        /// <returns></returns>
        T GetUtility<T>() where T : class, IUtility;

        /// <summary>
        /// 发送执行指令
        /// </summary>
        /// <typeparam name="T">指令</typeparam>
        void SendCommand<T>() where T : ICommand, new();

        /// <summary>
        /// 发送执行指令
        /// </summary>
        /// <typeparam name="T">指令</typeparam>
        /// <param name="command">指令</param>
        void SendCommand<T>(T command) where T : ICommand;

        /// <summary>
        /// 发送查询指令
        /// </summary>
        /// <typeparam name="TResult">查询结果</typeparam>
        /// <param name="query">查询指令</param>
        /// <returns>查询结果</returns>
        TResult SendQuery<TResult>(IQuery<TResult> query);

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        void SendEvent<T>() where T : new();

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        /// <param name="e">事件</param>
        void SendEvent<T>(in T e);

        /// <summary>
        /// 注册事件监听
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        /// <param name="onEvent">事件回调</param>
        /// <returns>注册句柄</returns>
        IUnRegister RegisterEvent<T>(IEventSystem.OnEventHandler<T> onEvent);

        /// <summary>
        /// 注销事件监听
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        /// <param name="onEvent">回调</param>
        void UnRegisterEvent<T>(IEventSystem.OnEventHandler<T> onEvent);
    }
}