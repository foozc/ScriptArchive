using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Event
{
    public interface IStateMachine<E>
    {
        void sendEvent(E evt);
    }
}
