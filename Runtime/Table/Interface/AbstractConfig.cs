namespace UnityLib.Data
{
    public class AbstractConfig<TKey> : IConfig<TKey>
        where TKey : struct
    {
        protected TKey id;
        public TKey Id => id;
    }
}