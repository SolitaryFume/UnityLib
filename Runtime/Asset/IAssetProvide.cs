using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace UnityLib.Asset
{
    public interface IAssetProvide
    {
        Task<T> LoadAsync<T>(string key) where T : Object;
        T Load<T>(string key) where T : Object;
    }
}
