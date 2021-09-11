using System.Collections;
using System.Collections.Generic;

namespace UnityLib.Data
{
    public abstract class Tb<TKey, TValue> : ITableConfig<TKey, TValue>
        where TKey : struct
        where TValue : IConfig<TKey>
    {
        protected Dictionary<TKey, TValue> m_dic = new Dictionary<TKey, TValue>();
        public TValue this[TKey key] => m_dic[key];

        public IEnumerable<TKey> Keys => m_dic.Keys;

        public IEnumerable<TValue> Values => m_dic.Values;

        public int Count => m_dic.Count;

        public bool ContainsKey(TKey key) => m_dic.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => m_dic.GetEnumerator();

        public bool TryGetValue(TKey key, out TValue value) => m_dic.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => m_dic.GetEnumerator();
    }
}

