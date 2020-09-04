using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Framework
{
    public class Container<K, T> where T : class, new()
    {
        protected Dictionary<K, T> elements = new Dictionary<K, T>();
        public int Count => elements.Count;
        public virtual T Get(K uid)
        {
            T element = null;
            lock (elements)
            {
                if (elements.TryGetValue(uid, out element) == false)
                {
                    return null;
                }
                return element;
            }
        }

        public delegate void CreateCallback(T value);

        public virtual T Create(K uid, CreateCallback callback)
        {
            T element = null;
            lock (elements)
            {
                element = Get(uid);
                if (element == null)
                {
                    element = new T();
                    Add(uid, element);
                    callback?.Invoke(element);
                }
            }
            return element;
        }

        public virtual T GetOrCreate(K uid, CreateCallback callback = null)
        {
            T element = null;
            lock (elements)
            {
                element = Get(uid);
                if (element == null)
                {
                    element = Create(uid, callback);
                }
                return element;
            }
        }
        public virtual T Add(K uid, T element)
        {
            lock (elements)
            {
                elements.Remove(uid);
                elements.Add(uid, element);
                return element;
            }
        }

        public virtual T Pop(K uid)
        {
            T element = null;
            lock (elements)
            {
                if (elements.TryGetValue(uid, out element) == true)
                {
                    elements.Remove(uid);
                    return element;
                }
                return null;
            }
            
        }

        public KeyValuePair<K,T>[] ToArray()
        {
            lock (elements)
            {
                return elements.ToArray();
            }
        }

        public T[] Values => elements.Values.ToArray();
    }
}
