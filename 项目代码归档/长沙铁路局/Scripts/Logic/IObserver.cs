using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Logic
{
    public interface IObserver
    {
        void update(Observable o , params object[] args);
    }
}
