using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
using UnityEngine;

namespace UnityLib.Asset
{
    public class AddrsAssetProvide : IAssetProvide
    {
        public AddrsAssetProvide()
        {
            
        }

        public T Load<T>(string key) where T : Object
        {
            return Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
        }

        public Task<T> LoadAsync<T>(string key) where T : Object
        {
            return Addressables.LoadAssetAsync<T>(key).Task;
        }
    }
}
