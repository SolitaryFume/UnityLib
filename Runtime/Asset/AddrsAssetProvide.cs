using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

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

        public void LoadScene(string key)
        {
            Addressables.LoadSceneAsync(key).WaitForCompletion();
        }

        public Task LoadSceneAsync(string key)
        {
            return Addressables.LoadSceneAsync(key).Task;
        }
    }
}
