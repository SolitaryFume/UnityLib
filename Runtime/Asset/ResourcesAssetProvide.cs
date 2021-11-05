using System.Threading.Tasks;
using Object = UnityEngine.Object;
using UnityEngine;

namespace UnityLib.Asset
{
    public class ResourcesAssetProvide : IAssetProvide
    {
        public T Load<T>(string key) where T : Object
        {
            return Resources.Load<T>(key);
        }

        public Task<T> LoadAsync<T>(string key) where T : Object
        {
            return Task.FromResult<T>(Resources.Load<T>(key));
        }

    }
}
