using UnityEngine.Networking;
namespace UnityLib.Web
{
    public static class UnityWebRequestAsyncOperationExtend
    {
        public static UnityWebRequestAsyncOperationAsync GetAwaiter(this UnityWebRequestAsyncOperation self)
        {
            return new UnityWebRequestAsyncOperationAsync(self);
        }
    }
}