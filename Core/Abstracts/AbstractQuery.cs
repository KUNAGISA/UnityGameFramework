using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    /// <summary>
    /// 查询数据基类
    /// 一般用于跨数据模块查询
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public abstract class AbstractQuery<TResult> : IQuery<TResult>
    {
        private IArchitecture m_Archiecture;

        TResult IQuery<TResult>.Do()
        {
            return OnDo();
        }

        protected abstract TResult OnDo();

        IArchitecture IBelongArchiecture.GetArchitecture()
        {
            return m_Archiecture;
        }

        void ICanSetArchiecture.SetArchiecture(IArchitecture architecture)
        {
            m_Archiecture = architecture;
        }
    }
}
