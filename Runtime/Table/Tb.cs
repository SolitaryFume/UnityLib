using System.Collections;
using System.Collections.Generic;

namespace UnityLib.Data
{
    public abstract class Table<TKey, TValue> : ITableConfig<TKey, TValue>
        where TValue : struct, IConfig<TKey>
        where TKey : struct
    {
        protected IReadOnlyDictionary<TKey, TValue> m_dir;
        public Table(TValue[] array)
        {
            var dir = new Dictionary<TKey, TValue>();
            foreach (var item in array)
            {
                dir.Add(item.Id, item);
            }
            m_dir = dir;
        }
        public TValue this[TKey key] => m_dir[key];

        public IEnumerable<TKey> Keys => m_dir.Keys;

        public IEnumerable<TValue> Values => m_dir.Values;

        public int Count => m_dir.Count;

        public bool ContainsKey(TKey key)=> m_dir.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()=> m_dir.GetEnumerator();

        public bool TryGetValue(TKey key, out TValue value) => m_dir.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()=>m_dir.GetEnumerator();
    }
}

