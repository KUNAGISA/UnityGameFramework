using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public interface ISystem : IBelongArchiecture, ICanSetArchiecture, ICanGetUtility, ICanGetModel, ICanRegisterEvent, ICanSendEvent, ICanSendQuery, ICanGetSystem
    {
        void InitSystem();
    }
}