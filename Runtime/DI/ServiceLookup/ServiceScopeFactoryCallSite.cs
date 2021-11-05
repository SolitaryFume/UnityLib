using System;

namespace UnityLib.DI
{
    internal class ServiceScopeFactoryCallSite : ServiceCallSite
    {
        public ServiceScopeFactoryCallSite() : base(ResultCache.None)
        {
        }

        public override Type ServiceType { get; } = typeof(IServiceScopeFactory);
        public override Type ImplementationType { get; } = typeof(ServiceProviderEngine);
        public override CallSiteKind Kind { get; } = CallSiteKind.ServiceScopeFactory;
    }
}
