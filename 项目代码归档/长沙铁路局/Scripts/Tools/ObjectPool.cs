using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using System.Net;

using Assets.Scripts.Log;

namespace Assets.Scripts.Tools
{
    public class ObjectPool<T> where T : IPoolObject
    {

        
        private int lockFlag = 0;

        private Queue<T> queue = new Queue<T>();
        private Func<T> func = null;
        private int capacity;
        private int size;

        private void enter()
        {
            //原子操作
            while (Interlocked.Exchange(ref lockFlag, 1) != 0)
            { 
                
            }
        }


        private void leave()
        {
            Thread.VolatileWrite(ref lockFlag,0);
        }


        public ObjectPool(int capacity, Func<T> func = default(Func<T>))
        {

            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }

            if (func == null)
            {
                func = () => (T)Activator.CreateInstance(typeof(T));
            }
            this.func = func;
            for (int i = 0; i < capacity; i++)
            {
                T t = func();
                t.reset();
                this.queue.Enqueue(t);
            }
            this.size = capacity;
            this.capacity = capacity;
        }

        public T rent()
        {
            lock (this)
            {
                if (this.queue.Count == 0)
                {
                    int ext = Math.Max(1, capacity / 2);
                    for (int i = 0; i < ext; i++)
                    {
                        T t = func();
                        t.reset();
                        this.queue.Enqueue(t);
                    }
                    this.capacity += ext;
                }
                this.size--;
                return this.queue.Dequeue();
            }
        }

        public void recycle(T t)
        {
            lock (this)
            {
                t.reset();
                this.queue.Enqueue(t);
                this.size++;
            }
        }


        public int getCapacity()
        {
            return this.capacity;
        }

        public int getSize()
        {
            return this.size;
        }
    }
}