using System;

namespace UnityLib.DI
{
    public interface ISupportRequiredService
    {
        object GetRequiredService(Type serviceType);
    }
}
