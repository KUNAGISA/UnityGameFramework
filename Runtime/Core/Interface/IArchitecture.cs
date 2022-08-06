using Framework.Internal.Operate;

namespace Framework
{
    public interface IArchitecture : IGetManager, IGetModel, IGetSystem, IGetUtility, ISendCommand, ISendEvent, ISendQuery
    {
        /// <summary>
        /// 注册或替换新Manager
        /// </summary>
        /// <typeparam name="T">Manager Class</typeparam>
        /// <param name="manager">Manager Instance</param>
        void RegisterManager<T>(T manager) where T : class, IManager;

        /// <summary>
        /// 注册或替换新系统模块
        /// </summary>
        /// <typeparam name="T">系统模块</typeparam>
        /// <param name="system">系统模块</param>
        void RegisterSystem<T>(T system) where T : class, ISystem;

        /// <summary>
        /// 注册或替换数据模块
        /// </summary>
        /// <typeparam name="T">数据模块</typeparam>
        /// <param name="model">数据模块</param>
        void RegisterModel<T>(T model) where T : class, IModel;

        /// <summary>
        /// 注册或替换工具模块
        /// </summary>
        /// <typeparam name="T">工具模块</typeparam>
        /// <param name="utility">工具模块</param>
        void RegisterUtility<T>(T utility) where T : class, IUtility;

        /// <summary>
        /// 注册事件监听
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        /// <param name="onEvent">事件回调</param>
        /// <returns>注册句柄</returns>
        IUnRegister RegisterEvent<T>(IEventSystem.OnEventHandler<T> onEvent) where T : struct;

        /// <summary>
        /// 注销事件监听
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        /// <param name="onEvent">回调</param>
        void UnRegisterEvent<T>(IEventSystem.OnEventHandler<T> onEvent) where T : struct;

        void Inject(object @object);
    }
}