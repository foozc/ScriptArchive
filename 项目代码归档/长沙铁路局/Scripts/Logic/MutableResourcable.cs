using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Logic
{
    public abstract class MutableResourcable <T> : IResourcable
    {
        private T mutableValue;
        public Resource getResource()
        {
            return getResource(this.mutableValue);
        }

        public void setMutableValue(T t)
        {
            this.mutableValue = t;
        }

        public abstract Resource getResource(T t);
    }
}
