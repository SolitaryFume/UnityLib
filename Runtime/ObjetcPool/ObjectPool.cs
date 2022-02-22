using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityLib.Pool
{
    public abstract class ObjectPool<T> where T : class
    {
        public abstract T Get();
        public abstract bool Return(T obj);
    }
}