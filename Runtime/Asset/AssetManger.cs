using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
using UnityLib.Pool;
using System.Threading;

namespace UnityLib.Asset
{
    public sealed class AssetManger
    {
        static IAssetProvide m_assetProvide;
        private static IAssetProvide assetProvide
        {
            get
            {
                if (m_assetProvide == null)
                {
                    m_assetProvide = ServiceMgr.GetService<IAssetProvide>();
                }
                return m_assetProvide;
            }
        }

        public static Task<T> LoadAsync<T>(string key) where T : Object
        {
            if (string.IsNullOrEmpty(key))
                Debug.LogError("key is null or empty !");
            return assetProvide.LoadAsync<T>(key);
        }

        public static T Load<T>(string key) where T : Object
        {
            if (string.IsNullOrEmpty(key))
                Debug.LogError("key is null or empty !");
            return assetProvide.Load<T>(key);
        }

        public static Object Load(string key)
        {
            if (string.IsNullOrEmpty(key))
                Debug.LogError("key is null or empty !");
            return Load<Object>(key);
        }

        public static async void LoadAsync(string key, Action<Object> onComplete,CancellationToken token)
        {
            if (string.IsNullOrEmpty(key))

                Debug.LogError("key is null or empty !");
            var task = LoadAsync<Object>(key);
            token.Register(()=>{
                task.Dispose();
            });
            var obj = await LoadAsync<Object>(key);
            onComplete?.Invoke(obj);
        }

        // public GameObject Create()
        // {
        //     throw new NotImplementedException();
        // }

        // public bool Return(GameObject obj)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
