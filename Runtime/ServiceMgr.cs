using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityLib.DI;

namespace UnityLib
{
    public sealed class ServiceMgr
    {
        private static ServiceProvider provider = null;

        public static void Init(IServiceCollection collection)
        {
            if(provider!=null)
                throw new Exception("重复初始化");
            provider = collection.BuildServiceProvider();
        }

        public static object GetService(Type serviceType)
        {
            if(provider==null)
                throw new Exception("ServiceMgr No Init");
            return provider.GetService(serviceType);
        }

        public static T GetService<T>()
        {
            if(provider==null)
                throw new Exception("ServiceMgr No Init");
            return provider.GetService<T>();
        }

        public static IServiceScope CreateScope()
        {
            if(provider==null)
                throw new Exception("ServiceMgr No Init");
            return provider.CreateScope();
        }
    }

}