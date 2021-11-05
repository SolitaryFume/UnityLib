using System;

namespace UnityLib.DI
{
    public interface IServiceProviderFactory<TContainerBuilder>
    {
        TContainerBuilder CreateBuilder(IServiceCollection services);
        IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder);
    }
}
