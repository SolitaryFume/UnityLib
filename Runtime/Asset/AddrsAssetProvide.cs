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
            // Addressables.InitializeAsync().WaitForCompletion();
        }

        public T Load<T>(string key) where T : Object
        {
            // Addressables.LoadSceneAsync
            return Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
        }

        public Task<T> LoadAsync<T>(string key) where T : Object
        {
            return Addressables.LoadAssetAsync<T>(key).Task;
        }
    }
}
