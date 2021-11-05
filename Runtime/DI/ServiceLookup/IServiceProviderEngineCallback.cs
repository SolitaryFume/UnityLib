
using System;

namespace UnityLib.DI
{
    internal interface IServiceProviderEngineCallback
    {
        void OnCreate(ServiceCallSite callSite);
        void OnResolve(Type serviceType, IServiceScope scope);
    }
}