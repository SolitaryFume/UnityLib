
using System;

namespace UnityLib.DI
{
    public class ServiceProviderOptions
    {
        internal static readonly ServiceProviderOptions Default = new ServiceProviderOptions();

        public bool ValidateScopes { get; set; }

        internal ServiceProviderMode Mode { get; set; } = ServiceProviderMode.Dynamic;
    }
}
