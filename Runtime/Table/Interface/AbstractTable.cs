using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace UnityLib.Data
{
    public abstract class AbstractTable<TKey, TValue> : ITableConfig<TKey, TValue>
        where TValue : IConfig<TKey>
        where TKey : struct
    {
        protected Dictionary<TKey, TValue> m_dir;
        public AbstractTable(IEnumerable<TValue> collecttion)
        {
            var dir = new Dictionary<TKey, TValue>();
            foreach (var item in collecttion)
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

        public virtual TValue GetValue(TKey key)
        {
            return this[key];
        }

        public bool TryGetValue(TKey key, out TValue value) => m_dir.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()=>m_dir.GetEnumerator();
    }
}

