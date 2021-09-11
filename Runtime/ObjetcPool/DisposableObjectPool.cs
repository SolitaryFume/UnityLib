using System;
using System.Threading;

namespace UnityLib.Pool
{
    public sealed class DisposableObjectPool<T> : DefaultObjectPool<T>, IDisposable where T : class
    {
        public DisposableObjectPool(IPooledObjectPolicy<T> policy):base(policy){}
        public DisposableObjectPool(IPooledObjectPolicy<T> policy, int maximumRetained): base(policy, maximumRetained){}

        public override void Return(T obj)
        {
            DisposeItem(obj);
            base.Return(obj);
        }

        public void Dispose()
        {
            for (var i = 0; i < _items.Length; i++)
            {
                DisposeItem(_items[i]);
                _items[i] = null;
            }
        }

        private static void DisposeItem(T item)
        {
           if(item is IDisposable disposable) 
           {
               disposable.Dispose();
           }
        }
    }
}