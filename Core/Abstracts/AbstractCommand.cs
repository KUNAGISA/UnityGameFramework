﻿
namespace Framework
{
    /// <summary>
    /// 数据增删改基类
    /// </summary>
    public abstract class AbstractCommand : ICommand
    {
        void ICommand.Execute()
        {
            OnExecute();
        }

        abstract protected void OnExecute();

        private IArchitecture m_Architecture;

        IArchitecture IBelongArchiecture.GetArchitecture()
        {
            return m_Architecture;
        }

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture)
        {
            m_Architecture = architecture;
        }
    }
}