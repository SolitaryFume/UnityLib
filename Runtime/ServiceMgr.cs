using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityLib
{
    public sealed class ServiceMgr
    {
        public static object GetService(Type serviceType)
        {
            return null;
        }

        public static T GetService<T>()
        {
            return default;
        }
    }

}