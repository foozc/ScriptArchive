using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Event
{
    public interface IObserver
    {
         void Response();

         void Init(IBaseModel ibm);
    }
}
