using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Assets.Scripts.Event
{
    public class StateMachine<S, E> : IStateMachine<E>
    {
        public delegate S OnStateChangeHandler(S state, E evt);

        protected class Key
        {
            S state;
            E evt;

            public Key(S state, E evt)
            {
                this.state = state;
                this.evt = evt;
            }

            public override bool Equals(object obj)
            {
                Key key = obj as Key;
                if (key == null)
                {
                    return false;
                }
                return state.Equals(key.state) && evt.Equals(key.evt);
            }

            public override int GetHashCode()
            {
                return state.GetHashCode() ^ evt.GetHashCode() + state.GetHashCode();
            }
        }


        protected Dictionary<Key, OnStateChangeHandler> rules = null;

        protected S last, current;

        protected bool processing = false;

        private static StateMachine<S, E> instance = null;

        private static Dictionary<object, StateMachine<S, E>> instances = null;


        public static StateMachine<S, E> getInstance()
        {
            if (instance == null)
            {
                instance = new StateMachine<S, E>();
            }
            return instance;
        }

        public static StateMachine<S, E> getInstance(object key)
        {
            if (instances == null)
            {
                instances = new Dictionary<object, StateMachine<S, E>>();
            }
            if (instances.ContainsKey(key))
            {
                return instances[key];
            }
            StateMachine<S, E> sm = new StateMachine<S, E>();
            instances.Add(key,sm);
            return sm;
        }


        protected StateMachine()
        {
            rules = new Dictionary<Key, OnStateChangeHandler>();
            last = current = default(S);
        }

        public void pushRule(S current, E evt, OnStateChangeHandler handler)
        {
            Key key = new Key(current ,evt);
            if (!rules.ContainsKey(key))
            {
                rules.Add(key, handler);
                return;
            }
            Log.Logger.warn(Log.Module.StateMachine, "already exist rule with (" + current + "," + evt + ")");
        }

        public void sendEvent(E evt)
        {
            Key key = new Key(current ,evt);
            if (rules.ContainsKey(key))
            {
                if (processing)
                {
                    processing = false;
                    Log.Logger.exception(Log.Module.StateMachine, "It is forbidden to send event when processing one. event:" + evt + ", state:" + current);
                }
                processing = true;
                S tmp = current;
                current = rules[key](current, evt);
                processing = false;
                return;
            }
            Log.Logger.exception(Log.Module.StateMachine, "Receive unexpected event:" + evt + " when state is:" + current + ", have you pushed the rule?");
        }

        public S getState()
        {
            return current;
        }
    }
}