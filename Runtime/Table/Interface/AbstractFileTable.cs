using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Linq;

namespace UnityLib.Data
{
    public abstract class AbstractFileTable<TKey, TValue> : ITableConfig<TKey, TValue>
        where TValue : IConfig<TKey>
        where TKey : struct
    {
        private int step;
        private byte[] buffer ;
        protected FileStream fileStream;
        protected bool AutoSave = false; 
        public AbstractFileTable()
        {}
        public AbstractFileTable(string filePtah,TableOption option)
        {
            if(string.IsNullOrEmpty(filePtah))
            {
                throw new ArgumentNullException(nameof(filePtah));
            }
            if(!File.Exists(filePtah))
            {
                throw new TableException($"no file : {filePtah}");
            }
            AutoSave = (option & TableOption.AutoSave)!=0;

           
            if((option & TableOption.LoadAll)!=0)//加载全部
            {
                var all = SerializeHelp.BytesToStructArray<TValue>(File.ReadAllBytes(filePtah));
                dir = new Dictionary<TKey, TValue>(all.Length);
                foreach (var item in all)
                {
                    dir.Add(item.Id,item);
                }
                keys = dir.Keys.ToList();
            }
            else
            {
                step =  Marshal.SizeOf<TValue>();
                buffer = new byte[step];
                fileStream = File.OpenRead(filePtah);
                var keySize =Marshal.SizeOf<TKey>();
                var keyBuffer = new byte[keySize];
                var length  = (int)fileStream.Length/step;
                Count = length;
                keys = new List<TKey>(length);
                dir = new Dictionary<TKey, TValue>(length);

                for (int i = 0; i < length; i++)//初始化索引
                {
                    fileStream.Position = i * step;
                    fileStream.Read(keyBuffer, 0, keySize);
                    unsafe
                    {
                        fixed (byte* p = &keyBuffer[0])
                        {
                            var intptr = (IntPtr)p;
                            var key = Marshal.PtrToStructure<TKey>(intptr);
                            keys.Add(key);
                        }
                    }
                }
            }


        }

        protected List<TKey> keys;
        public TValue this[TKey key]
        {
            get
            {
                if(AutoSave)
                {
                    return GetValue(key);
                }
                if(TryGetValue(key,out var result))
                    return result;
                throw new IndexOutOfRangeException($"No Find Config {typeof(TValue)} => key {key} !");
            }
        }

        protected Dictionary<TKey,TValue> dir;
        public IEnumerable<TKey> Keys => keys;

        public IEnumerable<TValue> Values => throw new System.NotImplementedException();

        public int Count {get;private set;}

        public bool ContainsKey(TKey key) => keys.Contains(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if(dir.TryGetValue(key,out value))
            {
                return true;
            }

            var index = keys.BinarySearch(key);
            if(index<0)
            {
                Debug.LogError($"no Find {key}");
                value = default;
                return false;
            }
            else
            {
                value = ReadValue(index);
                return true;
            }
        }

        private unsafe TValue ReadValue(int index)
        {
            fileStream.Position = index*step;
            fileStream.Read(buffer,0,step);
            fixed(byte* p = &buffer[0])
            {
                var ptr = (IntPtr)p;
                return Marshal.PtrToStructure<TValue>(ptr);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public TValue GetValue(TKey key)
        {
            if(!TryGetValue(key,out var result))
            {
                throw new TableException("No Find Value");
            }
            if(!dir.ContainsKey(key))
            {
                dir.Add(key,result);
            }
            return result;
        }

        ~AbstractFileTable()
        {
            fileStream.Close();
        }
    }
}

