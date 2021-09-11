using UnityEngine;

namespace UnityLib.Data
{
    public interface IConfig<TKey> where TKey : struct
    {
        TKey Id { get; }
    }

}