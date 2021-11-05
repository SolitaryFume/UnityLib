using System;

namespace UnityLib.DI
{
    public delegate object ObjectFactory(IServiceProvider serviceProvider, object[] arguments);
}