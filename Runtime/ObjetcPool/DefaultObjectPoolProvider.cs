using System;

namespace UnityLib.Pool
{

    public class DefaultObjectPoolProvider : ObjectPoolProvider
    {
        public int MaximumRetained { get; set; } = Environment.ProcessorCount * 2;
        public override ObjectPool<T> Create<T>(IPooledObjectPolicy<T> policy)
        {
            if(policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            if(typeof(IDisposable).IsAssignableFrom(typeof(T)))
            {
                return new DisposableObjectPool<T>(policy,MaximumRetained);
            }

            return new DefaultObjectPool<T>(policy,MaximumRetained);
        }
    }
}