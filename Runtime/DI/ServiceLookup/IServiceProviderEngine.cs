
using System;

namespace UnityLib.DI
{
    internal interface IServiceProviderEngine : IDisposable, IServiceProvider
    {
        IServiceScope RootScope { get; }
    }
}