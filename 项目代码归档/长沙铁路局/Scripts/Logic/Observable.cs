using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine ;

using Assets.Scripts.Log;

namespace Assets.Scripts.Logic
{
    public class Observable : MonoBehaviour
    {

        public delegate void SetChangedHandler();

        public delegate void ClearChangedHandler();

        private bool changed = false;
        private LinkedList<IObserver> obs = null;

        public Observable()
        {
            obs = new LinkedList<IObserver>();
        }

        //User with IObserverable
        public Observable(out SetChangedHandler handler1, out ClearChangedHandler handler2)
            : this()
        {
            handler1 = setChanged;
            handler2 = clearChanged;
        }

        //TODO sync
        public void addObserver(IObserver o)
        {
            if (o == null)
                throw new NullReferenceException();
            if (!obs.Contains(o))
            {
                obs.AddLast(o);
            }
        }

        //TODO sync
        public void deleteObserver(IObserver o)
        {
            if (obs.Contains(o))
            { 
                obs.Remove(o);
            }
        }

        public void notifyObservers()
        {
            notifyObservers(null);
        }

        //TODO sync
        public void notifyObservers(params object[] args)
        {
            if (!changed)
                return;
            foreach (IObserver i in obs)
            {
                i.update(this, args);
            }
        }

        //TODO sync
        protected void setChanged()
        {
            changed = true;
        }

        //TODO sync
        protected void clearChanged()
        {
            changed = false;
        }

        //TODO sync
        public bool hasChanged()
        {
            return changed;
        }

        //TODO sync
        //Returns the number of observers of this Observable object.
        public int countObservers()
        {
            return obs.Count;
        }
    }
}
