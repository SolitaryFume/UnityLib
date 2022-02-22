using System.Diagnostics;
using System.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace UnityLib.Pool
{
    public class DefaultObjectPool<T> : ObjectPool<T> where T : class
    {
        private protected readonly T[] _items;

        private int _count; 
        private protected readonly IPooledObjectPolicy<T> _policy;
        private protected readonly bool _isDefaultPolicy;
        private protected readonly PooledObjectPolicy<T> _fastPolicy;

        public DefaultObjectPool(IPooledObjectPolicy<T> policy):this(policy,Environment.ProcessorCount*2)
        {

        }

        public DefaultObjectPool(IPooledObjectPolicy<T> policy,int maximumRetained)
        {
            _policy = policy??throw new ArgumentNullException(nameof(policy));
            _fastPolicy = policy as PooledObjectPolicy<T>;
            _isDefaultPolicy = IsDefaultPolicy();

            _items = new T[maximumRetained-1];

            bool IsDefaultPolicy()
            {
                var type = policy.GetType();
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DefaultPooledObjectPolicy<>);
            }
        }

        public override T Get()
        {
            T item;
            if(_count>0)
            {
                item = _items[--_count];
                _items[_count] = null;
            }
            else
            {
                item = Create();
            }
            return item;
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private T Create()=>_fastPolicy?.Create()??_policy.Create();

        public override bool Return(T obj)
        {
            if(obj==null)
                return false;
            if(_count>_items.Length)
            {
                return false;
            }
            else
            {
                _items[_count++] = obj;
                return false;
            }
        }
    }
}