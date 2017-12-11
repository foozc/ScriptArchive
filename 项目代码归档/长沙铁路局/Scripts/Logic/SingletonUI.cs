using System;
using UnityEngine;


using Assets.Scripts.Log;


namespace Assets.Scripts.Logic
{
    public class SingletonUI<T> : UIModel where T : SingletonUI<T>
    {
        private static T instance;

        public static T getInstance()
        {
            if (!instance)
            {
                instance = (T)GameObject.FindObjectOfType(typeof(T));
                if (!instance)
                {
                    Log.Logger.error(Module.Framework, "There needs to be one active " + typeof(T) + " script on a GameObject in your scene.");
                }
            }
            return instance;
        }
    }
}
