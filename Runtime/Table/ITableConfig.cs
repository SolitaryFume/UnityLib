using System.Collections.Generic;

namespace UnityLib.Data
{
    public interface ITableConfig<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        where TKey : struct
        where TValue : IConfig<TKey>
    {

    }
}