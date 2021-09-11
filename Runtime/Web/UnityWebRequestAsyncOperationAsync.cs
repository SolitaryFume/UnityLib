using UnityEngine.Networking;
using System.Runtime.CompilerServices;
using System;
namespace UnityLib.Web
{
    public struct UnityWebRequestAsyncOperationAsync : IAsync<DownloadHandler>
    {
        private UnityWebRequestAsyncOperation operation;
        public UnityWebRequestAsyncOperationAsync(UnityWebRequestAsyncOperation operation)
        {
            this.operation = operation;
        }

        public DownloadHandler GetResult() => operation.webRequest.downloadHandler;
        public bool IsCompleted => operation.isDone;

        public void OnCompleted(Action continuation)
        {
            operation.completed += _ => continuation();
        }
    }
}