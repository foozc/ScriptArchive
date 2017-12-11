using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


namespace Assets.Scripts.Event
{

    /// <summary>
    /// 观察者基类
    /// </summary>
    public abstract class ObserverBase :MonoBehaviour, IObserver
    {


        #region IObserver 成员

        public virtual void Response()
        {
            throw new NotImplementedException();
        }

        public virtual void Init(IBaseModel ibm)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
